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
        private static readonly string selectAllQuery =
            "SELECT * FROM ContractFormula ";

        private static readonly string insertCommand =
                  @"INSERT INTO ContractFormula 
                     (Description, MaxUsageHoursPerPeriod, PeriodInMonths, NoticePeriodInMonths, Price)
                    VALUES (@Description, @MaxUsage, @Period, @NoticePeriod, @Price)";

        private static readonly string updateCommand =
                  @"UPDATE ContractFormula 
                    SET Description = @Description, MaxUsageHoursPerPeriod = @MaxUsage, 
                        PeriodInMonths = @Period, NoticePeriod = @NoticePeriod, Price = @Price
                    WHERE Id = ";

        public static IList<ContractFormula> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<ContractFormula>(selectAllQuery)
                                 .ToList<ContractFormula>();
            }
        }

        public static ContractFormula GetById(int contractFormulaId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<ContractFormula>(
                    selectAllQuery + "WHERE Id = @ContractFormulaId",
                    new { ContractFormulaId = contractFormulaId }
                ).SingleOrDefault();
            }
        }

        public static int Save(ContractFormula contractFormula)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(
                    isNew(contractFormula) ? insertCommand : updateCommand,
                    contractFormula
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        private static bool isNew(ContractFormula contractFormula)
        {
            return contractFormula.Id == 0;
        }

        public static int Delete(ContractFormula contractFormula)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(
                    "DELETE FROM ContractFormula WHERE Id = @Id",
                    contractFormula
                );
            }
        }
    }
}
