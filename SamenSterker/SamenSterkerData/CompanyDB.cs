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
        //connection with Company Table
        public static IEnumerable<Company> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Company";
                return connection.Query<Company>(query);
            }
        }

        public static Company GetById(int companyId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Company " +
                                     "WHERE Id = @CompanyId";

                return connection.Query<Company>(query, 
                       new { CompanyId = companyId }).SingleOrDefault();
            }
        }


        public static int save(Company company)
        {
            const string insertCommand =
                  "INSERT INTO Company " +
                  "(Id, Name, Street, Zipcode, City, Country, Email, Phone) " +  
                  "VALUES (@Id, @Name, @Street, @Zipcode, @City, @Country, " +
                  "@Email, @Phone)";


            const string updateCommand = 
                  "UPDATE Company " +
                  "SET Id = @Id, Name = @Name, Street = @Street, " +
                  "Zipcode = @Zipcode, City = @City, Country = @Country, " +
                  "Email = @Email, Phone = @Phone"; 

            bool isNew = company.Id == 0;
            string saveCommand =  isNew ? insertCommand : updateCommand;

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {        
                int rowsAffected = connection.Execute(saveCommand, company);
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        public static int Delete(Company company)
        {
            const string deleteCommand = 
                  "DELETE FROM Company WHERE Id = @CompanyId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {                 
                return connection.Execute(deleteCommand, 
                    new { CompanyId = company.Id });
            }
        }
    }
}
