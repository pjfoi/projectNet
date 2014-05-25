using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class LocationDB
    {
        public static IEnumerable<Location> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Location";
                return connection.Query<Location>(query);
            }
        }

        public static Location GetById(int locationId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Location " +
                                     "WHERE Id = @LocationId";

                return connection.Query<Location>(query,
                       new { LocationId = locationId }).SingleOrDefault();
            }
        }


        public static int save(Location location)
        {
            const string insertCommand =
                  "INSERT INTO Location " +
                  "(Id, Name)" +
                  "VALUES (@Id, @Name)";


            const string updateCommand =
                  "UPDATE Location " +
                  "SET Id = @Id, Name = @Name";

            bool isNew = location.Id == 0;
            string saveCommand = isNew ? insertCommand : updateCommand;

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(saveCommand, location);
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        public static int Delete(Location location)
        {
            const string deleteCommand =
                  "DELETE FROM Location WHERE Id = @LocationId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteCommand,
                    new { LocationId = location.Id });
            }
        }
    }
}
