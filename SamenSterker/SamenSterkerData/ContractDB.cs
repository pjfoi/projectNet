using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    class ContractDB
    {
        public static IEnumerable<Contract> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Contract";
                return connection.Query<Contract>(query);
            }
        }

        public static Company GetById(int contractId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM Contract " +
                                     "WHERE Id = @ContractId";

                return connection.Query<Company>(query,
                       new { ContractId = contractId }).SingleOrDefault();
            }
        }


        public static int save(Contract contract)
        {
            const string insertCommand =
                  "INSERT INTO Contract " +
                  "(Id, Number, StartDate, EndDate, CompanyId, ContractFormulaId) " +
                  "VALUES (@Id, @Number, @StartDate, @EndDate, @CompanyId, @ContractFormulaId)";


            const string updateCommand =
                  "UPDATE Company " +
                  "SET Id = @Id, Number = @Number, StartDate = @StartDate ," +
                  "EndDate = @EndDate, CompanyId = @CompanyId, ContractFormulaId = @ContractFormulaId";

            bool isNew = contract.Id == 0;
            string saveCommand = isNew ? insertCommand : updateCommand;

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(saveCommand, contract);
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        public static int Delete(Contract contract)
        {
            const string deleteCommand =
                  "DELETE FROM Contract WHERE Id = @ContractId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteCommand,
                    new { ContractId = contract.Id });
            }
        }
    }
}
