using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class CompanyDB
    {
        private static readonly string selectAllQuery = 
            "SELECT * FROM Company";

        private static readonly string insertCommand =
            @"INSERT INTO Company 
                (Name, Street, Zipcode, City, Country, Email, Phone, Employees) 
              VALUES (@Name, @Street, @Zipcode, @City, @Country,
                @Email, @Phone, @Employees)";

        private static readonly string updateCommand =
            @"UPDATE Company 
              SET Name = @Name, Street = @Street, Zipcode = @Zipcode, City = @City,
                  Country = @Country, Email = @Email, Phone = @Phone, Employees = @Employees
              WHERE Id = @Id";

        private static readonly string deleteQuery =
            "DELETE FROM Company WHERE Id = @Id";

        public static IList<Company> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Company>(selectAllQuery)
                                 .ToList<Company>();
            }
        }

        public static Company GetById(int companyId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Company>(
                    selectAllQuery + " WHERE Id = @CompanyId",
                    new { CompanyId = companyId }
                ).SingleOrDefault();
            }
        }

        public static int Save(Company company)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {        
                int rowsAffected = connection.Execute(
                    isNew(company) ? insertCommand : updateCommand, 
                    company
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        private static bool isNew(Company company)
        {
            return company.Id == 0;
        }

        public static int Delete(Company company)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {                 
                return connection.Execute(deleteQuery, company);
            }
        }

        public static int Delete(IEnumerable<Company> companies)
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
                            param: companies,
                            transaction: transaction
                        );
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + exception,
                                                           "CompanyDB Delete");
                        return 0;
                    }
                }
            }
        }
    }
}
