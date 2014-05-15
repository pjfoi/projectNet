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
        public static List<Contract> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = @"SELECT ct.*, cmp.*, f.* 
                                     FROM Contract ct
                                     LEFT JOIN Company cmp ON ct.CompanyId = cmp.Id
                                     LEFT JOIN ContractFormula f ON ct.ContractFormulaId = f.Id";
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                    query,
                    // set the company and the formula properties of the contract
                    (ct, cmp, f) =>
                        {
                            ct.Company = cmp;
                            ct.Formula = f;
                            return ct;
                        }
                    )
                    .ToList();
            }
        }

        public static Contract GetById(int contractId)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                const string query = @"SELECT ct.*, cmp.*, f.* FROM Contract ct
                                     LEFT JOIN Company cmp ON ct.CompanyId = cmp.Id
                                     LEFT JOIN ConctactFormula f ON ct.ContractFormulaId = f.Id
                                     WHERE Id = @ContractId";

                return connection.Query<Contract, Company, ContractFormula, Contract>(
                    query,
                    // set the company and the formula properties of the contract
                    (ct, cmp, f) => 
                        {
                            ct.Company = cmp;
                            ct.Formula = f;
                            return ct;
                        },
                    new { ContractId = contractId }) 
                    .SingleOrDefault();
            }
        }

        // TODO 
        // id of company and contractformula (Contract.CompanyId == Contract
        public static int save(Contract contract)
        {
            const string insertCommand =
                  @"INSERT INTO Contract 
                  (Id, Number, StartDate, EndDate, CompanyId, ContractFormulaId) 
                  VALUES (@Id, @Number, @StartDate, @EndDate, @CompanyId, @ContractFormulaId)";


            const string updateCommand =
                  @"UPDATE Company 
                  SET Id = @Id, Number = @Number, StartDate = @StartDate ,
                  EndDate = @EndDate, CompanyId = @CompanyId, ContractFormulaId = @ContractFormulaId";

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
