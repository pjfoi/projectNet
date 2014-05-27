using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class UserDB
    {
        private static readonly string selectAllQuery =
            @"SELECT u.*, c.* FROM [User] u
              LEFT JOIN Company c ON u.CompanyId = c.Id ";

        private static readonly string deleteQuery =
            "DELETE FROM [User] WHERE id = @Id";

        public static List<User> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<User, Company, User>(
                    selectAllQuery, Mapper
                ).ToList<User>();
            }
        }

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

        public static int Delete(User user)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteQuery, user);
            }
        }

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
