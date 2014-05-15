using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class ContractFormulaDB
    {
        public static IEnumerable<ContractFormula> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM ContractFormula";
                return connection.Query<ContractFormula>(query);
            }
        }

        public static ContractFormula GetById(int contractFormulaId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = "SELECT * FROM ContractFormula " +
                                     "WHERE Id = @ContractFormulaId";

                return connection.Query<ContractFormula>(query,
                       new { ContractFormulaId = contractFormulaId }).SingleOrDefault();
            }
        }


        public static int save(ContractFormula contractformula)
        {
            const string insertCommand =
                  "INSERT INTO ContractFormula " +
                  "(Id, Description, MaxUsageHoursPerPeriod, PeriodInMonths, NoticePeriodInMonths, Price) " +
                  "VALUES (@Id, @Description, @MaxUsage, @Period, @NoticePeriod, @Price)";


            const string updateCommand =
                  "UPDATE ContractFormula " +
                  "SET Id = @Id, Description = @Description, MaxUsageHoursPerPeriod = @MaxUsage, " +
                  "PeriodInMonths = @Period, NoticePeriod = @NoticePeriod, Price = @Price";

            bool isNew = contractformula.Id == 0;
            string saveCommand = isNew ? insertCommand : updateCommand;

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(saveCommand, contractformula);
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        public static int Delete(ContractFormula contractformula)
        {
            const string deleteCommand =
                  "DELETE FROM ContractFormula WHERE Id = @ContractFormulaId";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(deleteCommand,
                    new { ContractFormulaId = contractformula.Id });
            }
        }
    }
}
