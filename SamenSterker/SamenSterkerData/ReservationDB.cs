using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    class ReservationDB
    {
        public static IEnumerable<Reservation> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Reservation";
                return connection.Query<Reservation>(query);
            }
        }

        public static Reservation GetById(int reservationId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Reservation " +
                                     "WHERE Id = @ReservationId";

                return connection.Query<Reservation>(query,
                       new { ReservationId = reservationId }).SingleOrDefault();
            }
        }


        public static int save(Reservation reservation)
        {
            const string insertCommand =
                  "INSERT INTO Reservation " +
                  "(Id, Number, StartDate, EndDate, CompanyId, LocationId, CreateId) " +
                  "VALUES (@Id, @Number, @StartDate, @EndDate, @ComanyId, @LocationId, " +
                  "@CreateId)";


            const string updateCommand =
                  "UPDATE Reservation " +
                  "SET Id = @Id, Number = @Number, StartDate = @StartDate, " +
                  "EndDate = @EndDate, CompanyId = @CompanyId, LocationId = @LocationId, " +
                  "CreateId = @CreateId";

            bool isNew = reservation.Id == 0;
            string saveCommand = isNew ? insertCommand : updateCommand;

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(saveCommand, reservation);
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        public static int Delete(Reservation reservation)
        {
            const string deleteCommand =
                  "DELETE FROM Reservation WHERE Id = @ReservationId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteCommand,
                    new { ReservationId = reservation.Id });
            }
        }
    }
}
