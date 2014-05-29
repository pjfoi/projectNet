using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SamenSterkerData
{
    /// <summary>
    /// Database interaction for locations.
    /// </summary>
    public class LocationDB
    {
        private static readonly string selectAllQuery =
            "SELECT * FROM Location ";

        private static readonly string insertCommand =
            @"INSERT INTO Location (Id, Name)
              VALUES (@Id, @Name)";

        private static readonly string updateCommand =
            @"UPDATE Location 
              SET Name = @Name
              WHERE Id = @Id";

        /// <summary>
        /// Get all the locations.
        /// </summary>
        /// <returns>All the locations.</returns>
        public static IEnumerable<Location> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Location>(selectAllQuery);
            }
        }

        /// <summary>
        /// Get the location with the specified id.
        /// </summary>
        /// <param name="locationId">The id of the requested location.</param>
        /// <returns>The location if it exists.</returns>
        public static Location GetById(int locationId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Location>(
                    sql: selectAllQuery + " WHERE Id = @LocationId",
                    param: new { LocationId = locationId }
                ).SingleOrDefault();
            }
        }

        /// <summary>
        /// Save the specified location.
        /// </summary>
        /// <param name="location">The location to be saved.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Save(Location location)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(
                    sql: isNew(location) ? insertCommand : updateCommand,
                    param: location
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        private static bool isNew(Location location)
        {
            return location.Id == 0;
        }

        /// <summary>
        /// Delete the specified location.
        /// </summary>
        /// <param name="location">The location to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(Location location)
        {
            const string deleteCommand =
                  "DELETE FROM Location WHERE Id = @LocationId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(
                    sql: deleteCommand,
                    param: location
                );
            }
        }
    }
}
