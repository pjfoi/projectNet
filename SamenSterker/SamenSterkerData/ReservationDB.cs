using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{

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

        public static List<Reservation> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Reservation, Location, Company, Reservation>(
                    selectAllQuery, ReservationMapper
                ).ToList<Reservation>();
            }
        }

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

        public static int Save(Reservation reservation)
        {
            // set LocationId if Location is specifified
            if (reservation.LocationId == 0 && reservation.Location != null)
            {
                reservation.LocationId = reservation.Location.Id;
            }

            // set CompanyId if Company is specifified
            if (reservation.CompanyId == 0 && reservation.Company != null)
            {
                reservation.CompanyId = reservation.Company.Id;
            }

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

        public static int Delete(Reservation reservation)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteQuery, reservation);
            }
        }

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
                if (contract.Formula.MaxUsageHoursPerPeriod > 0)
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
            const string selectConflictingReservations =
                @"SELECT COUNT(*) FROM Reservation
                  WHERE LocationId = @LocationId
                    AND (@StartDate BETWEEN StartDate AND EndDate
                        OR @EndDate BETWEEN StartDate And EndDate)";
                  //AND Id != @Id // for updating a reservation ??
            int nbConflicts = connection.Query<int>(
                selectConflictingReservations, reservation
            ).Single();
            return nbConflicts == 0;
        }

        private static int GetMinutesUsedOfContract(
            Contract contract, SqlConnection connection)
        {
            const string selectTotalMinutes =
                @"SELECT SUM( DATEDIFF(mi, r.StartDate, r.EndDate) )
                  FROM Reservations r
                  WHERE r.CompanyId = @CompanyId
                    AND r.StartDate BETWEEN @StartDate AND @EndDate
                    AND r.EndDate BETWEEN @StartDate AND @EndDate";
                //AND Id != @ReservationId // for updating a reservation ??
            int nbMinutes = connection.Query<int>(
                selectTotalMinutes, contract
            ).Single();
            return nbMinutes;
        }

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