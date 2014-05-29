using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SamenSterkerData
{
    /// <summary>
    /// Database interaction for user models.
    /// </summary>
    public class UserDB
    {
        // To create users, use WebSecurity.CreateUserAndAccount()

        private static readonly string selectAllQuery =
            @"SELECT u.*, c.* FROM [User] u
              LEFT JOIN Company c ON u.CompanyId = c.Id ";

        private static readonly string deleteQuery =
            "DELETE FROM [User] WHERE id = @Id";

        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>All the users</returns>
        public static List<User> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<User, Company, User>(
                    selectAllQuery, Mapper
                ).ToList<User>();
            }
        }

        /// <summary>
        /// Get all the users of the specified company.
        /// </summary>
        /// <param name="company">The company of which the users
        /// are requested.</param>
        /// <returns>The users of the specified company.</returns>
        public static List<User> GetFromCompany(Company company)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<User, Company, User>(
                    sql: selectAllQuery + "WHERE CompanyId = @CompanyId",
                    map: Mapper,
                    param: new { CompanyId = company.Id }
                ).ToList<User>();
            }
        }

        /// <summary>
        /// Get the user with the specified id.
        /// </summary>
        /// <param name="id">The id of the requested user</param>
        /// <returns>The user if it exists</returns>
        public static User GetById(int id)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<User, Company, User>(
                    sql: selectAllQuery + "WHERE Id = @Id",
                    map: Mapper,
                    param: new { Id = id }
                ).SingleOrDefault();
            }
        }

        /// <summary>
        /// Get the user with the specified username.
        /// </summary>
        /// <param name="username">The username of the requested user</param>
        /// <returns>The user if it exists</returns>
        public static User GetByUsername(string username)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<User, Company, User>(
                    sql: selectAllQuery + "WHERE UserName = @Username",
                    map: Mapper,
                    param: new { Username = username }
                ).SingleOrDefault();
            }
        }

        /// <summary>
        /// Delete the specified user.
        /// </summary>
        /// <param name="user">The user to be deleted</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(User user)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteQuery, user);
            }
        }

        /// <summary>
        /// Delete the specified users.
        /// </summary>
        /// <param name="users">The users to be deleted</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(IEnumerable<User> users)
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
                            param: users,
                            transaction: transaction
                        );
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + exception,
                                                           "UserDB Delete");
                        return 0;
                    }
                }
            }
        }

        private static bool isNew(User user)
        {
            return user.Id == 0;
        }

        private static User Mapper(User user, Company company)
        {
            user.Company = company;
            return user;
        }

    }
}
