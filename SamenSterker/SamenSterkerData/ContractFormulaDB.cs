using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SamenSterkerData
{
    /// <summary>
    /// Database interaction for contract formulas
    /// </summary>
    public class ContractFormulaDB
    {
        private static readonly string selectAllQuery =
            "SELECT * FROM ContractFormula ";

        private static readonly string insertCommand =
                  @"INSERT INTO ContractFormula 
                     (Description, MaxUsageHoursPerPeriod, PeriodInMonths, NoticePeriodInMonths, Price)
                    VALUES (@Description, @MaxUsageHoursPerPeriod, @PeriodInMonths, @NoticePeriod, @Price)";

        private static readonly string updateCommand =
                  @"UPDATE ContractFormula 
                    SET Description = @Description, MaxUsageHoursPerPeriod = @MaxUsageHoursPerPeriod, 
                        PeriodInMonths = @PeriodInMonths, NoticePeriod = @NoticePeriod, Price = @Price
                    WHERE Id = @Id";

        /// <summary>
        /// Get all the contract formulas.
        /// </summary>
        /// <returns>All the contract formulas.</returns>
        public static IList<ContractFormula> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<ContractFormula>(selectAllQuery)
                                 .ToList<ContractFormula>();
            }
        }

        /// <summary>
        /// Get the contract formula with the specified id.
        /// </summary>
        /// <param name="contractFormulaId">The id of the requested 
        /// contract formula</param>
        /// <returns>The requested contract formula if it exists.</returns>
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

        /// <summary>
        /// Save the specified contract formula.
        /// </summary>
        /// <param name="contractFormula">The contract formula to be saved.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Save(ContractFormula contractFormula)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(
                    sql: isNew(contractFormula) ? insertCommand : updateCommand,
                    param: contractFormula
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        private static bool isNew(ContractFormula contractFormula)
        {
            return contractFormula.Id == 0;
        }

        /// <summary>
        /// Delete the specified contract formula.
        /// </summary>
        /// <param name="contractFormula">The contract formula to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(ContractFormula contractFormula)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(
                    sql: "DELETE FROM ContractFormula WHERE Id = @Id",
                    param: contractFormula
                );
            }
        }
    }
}
