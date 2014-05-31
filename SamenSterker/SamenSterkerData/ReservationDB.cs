using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SamenSterkerData
{
    /// <summary>
    /// Database interaction for reservation models.
    /// </summary>
    public class ReservationDB
    {
        private static readonly string selectAllQuery =
            @"SELECT r.*, l.*, c.* FROM Reservation r
              LEFT JOIN Location l ON r.LocationId = l.Id 
              LEFT JOIN Company c ON r.CompanyId = c.Id ";

        private static readonly string insertCommand =
            @"INSERT INTO Reservation 
                (Number, StartDate, EndDate, CompanyId, LocationId, CreateDate) 
              VALUES (@Number, @StartDate, @EndDate, @CompanyId, @LocationId, 
                      SYSDATETIME())";

        private static readonly string updateCommand =
              @"UPDATE Reservation 
                SET Number = @Number, StartDate = @StartDate, EndDate = @EndDate, 
                    CompanyId = @CompanyId, LocationId = @LocationId
                WHERE Id = @Id";

        private static readonly string deleteQuery =
            "DELETE FROM Reservation WHERE Id = @Id";

        /// <summary>
        /// Get all the reservations.
        /// </summary>
        /// <returns>All the reservations.</returns>
        public static List<Reservation> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Reservation, Location, Company, Reservation>(
                    selectAllQuery, ReservationMapper
                ).ToList<Reservation>();
            }
        }

        /// <summary>
        /// Get the reservations on the specified date.
        /// </summary>
        /// <param name="date">The date of which the reservations are 
        /// requested</param>
        /// <returns>All the reservation on the specified date</returns>
        public static List<Reservation> GetAllOnDate(DateTime date)
        {
            string query = selectAllQuery +
                @"WHERE r.StartDate >= @Date AND r.StartDate < (@Date + 1)
                     OR r.EndDate >= @Date AND r.EndDate < (@Date + 1)
                  ORDER BY l.Name, r.StartDate";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Reservation, Location, Company, Reservation>(
                    query, ReservationMapper, new { Date = date }
                ).ToList<Reservation>();
            }
        }

        /// <summary>
        /// Get all the reservations of the specified company.
        /// </summary>
        /// <param name="company">The company of which the reservatios
        /// are requested.</param>
        /// <returns>The reservations of the company.</returns>
        public static List<Reservation> GetFromCompany(Company company)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Reservation, Location, Company, Reservation>(
                    sql: selectAllQuery + "WHERE CompanyId = @CompanyId",
                    map: ReservationMapper,
                    param: new { CompanyId = company.Id }
                ).ToList<Reservation>();
            }
        }

        /// <summary>
        /// Get the reservation with the specified id.
        /// </summary>
        /// <param name="reservationId">The id of the requested reservation</param>
        /// <returns>The reservation if it exists.</returns>
        public static Reservation GetById(int reservationId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Reservation, Location, Company, Reservation>(
                    sql: selectAllQuery + "WHERE Id = @ReservationId",
                    map: ReservationMapper, 
                    param: new { ReservationId = reservationId }
                ).SingleOrDefault();
            }
        }

        /// <summary>
        /// Save the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation to be saved.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Save(Reservation reservation)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(
                    sql: isNew(reservation) ? insertCommand : updateCommand,
                    param: reservation
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        /// <summary>
        /// Delete the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(Reservation reservation)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteQuery, reservation);
            }
        }

        /// <summary>
        /// Delete the specified reservations.
        /// </summary>
        /// <param name="reservation">The reservations to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(IEnumerable<Reservation> reservations)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int rowsAffected = connection.Execute(
                            sql: deleteQuery,
                            param: reservations,
                            transaction: transaction
                        );
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + exception,
                                                           "ReservationDB Delete");
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Is the specified reservation valid ?
        ///  - Does a contract exists for the selected time ?
        ///  - Is the selected location free at the selected time ?
        ///  - Is the limit of the contract not exceeded ?
        /// </summary>
        /// <param name="reservation">The reservation to be checked</param>
        /// <returns>If the reservation is valid</returns>
        public static bool IsReservationPossible(Reservation reservation)
        {
            // a contract exists for the time of the reservation
            Contract contract = ContractDB.GetContractForReservation(reservation);
            if (contract == null)
            {
                throw new InvalidReservationException(
                    "Uw bedrijf heeft op dat moment geen geldig contract"
                );
            }

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                // the selected location is free for the selected time
                if (! IsLocationFree(reservation, connection))
                {
                    throw new LocationOccupiedException(
                        "De gekozen locatie is niet vrij tijdens de gekozen periode"
                    );
                }

                // is the limit of the contract formula not exceeded 
                int limit = contract.Formula.MaxUsageHoursPerPeriod;
                if (limit > 0)
                {
                    TimeSpan timeUsed = new TimeSpan(0, GetMinutesUsedOfContract(contract, connection), 0);
                    TimeSpan timeLeft = new TimeSpan(limit, 0, 0).Subtract(timeUsed);
                    TimeSpan timeReservation = reservation.EndDate.Subtract(reservation.StartDate);
                    if (timeReservation > timeLeft)
                    {
                        throw new InvalidReservationException(
                            String.Format("De reservatie past niet meer binnen de limiet van uw contract.\n" +
                                          "U hebt {0} uren en {1} minuten over.", timeLeft.Hours, timeLeft.Minutes)
                        );
                    }
                }

            }

            return true;
        }

        private static bool IsLocationFree(Reservation reservation, SqlConnection connection)
        {
            const string selectLocationIsFree =
                @"SELECT CAsT(
                    CASE WHEN ( EXISTS(
                      SELECT r.* FROM Reservation r
                      WHERE r.LocationId = @LocationId
                        AND (@StartDate BETWEEN r.StartDate AND r.EndDate
                            OR @EndDate BETWEEN r.StartDate AND r.EndDate)
                        AND r.Id != @Id
                    )) THEN 0 ELSE 1 END
                  AS BIT)";
            return connection.Query<bool>(
                selectLocationIsFree, reservation
            ).Single();
        }

        private static int GetMinutesUsedOfContract(
            Contract contract, SqlConnection connection)
        {
            const string selectTotalMinutes =
                @"SELECT COALESCE( SUM(DATEDIFF(mi, r.StartDate, r.EndDate)), 0)
                  FROM Reservation r
                  WHERE r.CompanyId = @CompanyId
                    AND r.StartDate BETWEEN @StartDate AND @EndDate
                    AND r.EndDate BETWEEN @StartDate AND @EndDate";
                    //AND r.Id != @ReservationId"; for update !!
            int nbMinutes = connection.Query<int>(
                selectTotalMinutes, contract //{ ReservationId =  }
            ).Single();
            return nbMinutes;
        }

        /// <summary>
        /// Are there any reservations for the specified contract.
        /// </summary>
        /// <param name="contract">The contract to check</param>
        /// <returns>Whether or not any reservations exist.</returns>
        public static bool ExistsReservationOfContract(Contract contract)
        {
            const string reservationExistsQuery =
                @"SELECT CAsT(
                    CASE WHEN ( EXISTS(
                      SELECT r.* FROM Reservation r
                      WHERE r.CompanyId = @CompanyId
                        AND (r.StartDate > SYSDATETIME()
                            OR r.EndDate > SYSDATETIME())
                        AND r.StartDate BETWEEN @StartDate AND @EndDate
                        AND r.EndDate BETWEEN @StartDate AND @EndDate
                    )) THEN 1 ELSE 0 END
                  AS BIT)";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<bool>(
                    reservationExistsQuery, contract
                ).Single();
            }
        }

        private static bool isNew(Reservation reservation)
        {
            return reservation.Id == 0;
        }

        private static Reservation ReservationMapper(
            Reservation reservation, Location location, Company company)
        {
            reservation.Location = location;
            reservation.Company = company;
            return reservation;
        }

    }
}