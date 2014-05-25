using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class ContractDB
    {
        private static readonly string selectAllQuery =
            @"SELECT ct.*, cmp.*, f.* 
              FROM Contract ct
              LEFT JOIN Company cmp ON ct.CompanyId = cmp.Id
              LEFT JOIN ContractFormula f ON ct.ContractFormulaId = f.Id ";

        public static List<Contract> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                         selectAllQuery, Mapper
                       ).ToList();
            }
        }

        public static Contract GetById(int contractId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                         selectAllQuery + "WHERE Id = @ContractId",
                         Mapper,
                         new { ContractId = contractId }
                       ).SingleOrDefault();
            }
        }

        private static Contract Mapper(Contract ct, Company cmp, ContractFormula f)
        {
            ct.Company = cmp;
            ct.Formula = f;
            return ct;
        }

        public static int Save(Contract contract)
        {
            if (contract.CompanyId == 0 && contract.Company != null)
            {
                contract.CompanyId = contract.Company.Id;
            }

            if (contract.ContractFormulaId == 0 && contract.Formula != null)
            {
                contract.ContractFormulaId = contract.Formula.Id;
            }

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                int rowsAffected = connection.Execute(
                    sql: GetSaveCommand(contract),
                    param: contract
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        private static string GetSaveCommand(Contract contract)
        {
            const string insertCommand =
                  @"INSERT INTO Contract 
                      (Number, StartDate, EndDate, CompanyId, ContractFormulaId) 
                    VALUES (@Number, @StartDate, @EndDate, @CompanyId, @ContractFormulaId)";

            const string updateCommand =
                  @"UPDATE Company 
                    SET Number = @Number, StartDate = @StartDate, EndDate = @EndDate, 
                        CompanyId = @CompanyId, ContractFormulaId = @ContractFormulaId
                    WHERE Id = @Id,";

            bool isNew = contract.Id == 0;
            return isNew ? insertCommand : updateCommand;
        }

        public static int Delete(Contract contract)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(
                    sql: "DELETE FROM Contract WHERE Id = @Id",
                    param: contract
                );
            }
        }
    }
}
