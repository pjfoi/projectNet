using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    /// <summary>
    /// Database interactions for contracts.
    /// </summary>
    public class ContractDB
    {
        private static readonly string selectAllQuery =
            @"SELECT ct.*, cmp.*, f.* 
              FROM Contract ct
              LEFT JOIN Company cmp ON ct.CompanyId = cmp.Id
              LEFT JOIN ContractFormula f ON ct.ContractFormulaId = f.Id ";

        private static readonly string insertCommand =
            @"INSERT INTO Contract 
                (Number, StartDate, EndDate, CompanyId, ContractFormulaId) 
            VALUES (@Number, @StartDate, @EndDate, @CompanyId, @ContractFormulaId)";

        private static readonly string updateCommand =
            @"UPDATE Contract 
            SET Number = @Number, StartDate = @StartDate, EndDate = @EndDate, 
                CompanyId = @CompanyId, ContractFormulaId = @ContractFormulaId
            WHERE Id = @Id";

        private static readonly string deleteQuery =
            "DELETE FROM Contract WHERE Id = @Id";

        /// <summary>
        /// Get all contracts.
        /// </summary>
        /// <returns>All contracts</returns>
        public static List<Contract> GetAll()
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                         selectAllQuery, Mapper
                       ).ToList();
            }
        }

        /// <summary>
        /// Get all contracts of the specified company.
        /// </summary>
        /// <param name="company">The company of which the contracts are requested</param>
        /// <returns>The contracts of the specified company.</returns>
        public static List<Contract> GetFromCompany(Company company)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                         sql: selectAllQuery + "WHERE CompanyId = @Id",
                         map: Mapper,
                         param: company
                       ).ToList<Contract>();
            }
        }

        /// <summary>
        /// Get the contract with the specified id.
        /// </summary>
        /// <param name="contractId">The id of the requested contract.</param>
        /// <returns>The contract if it exists.</returns>
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

        /// <summary>
        /// Save the specified contract if there is no conflicting contract.
        /// </summary>
        /// <param name="contract">The contract to be saved.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Save(Contract contract)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                if(ExistsConflictingContract(contract, connection))
                {
                    throw new InvalidContractException(
                        "Er bestaat al een contract tijdens de geselecteerde periode."
                    );
                }

                int rowsAffected = connection.Execute(
                    sql: isNew(contract) ? insertCommand : updateCommand,
                    param: contract
                );
                //SetIdentity<int>(connection, id => subCategory.Id = id);
                return rowsAffected;
            }
        }

        /// <summary>
        /// Stop the specified contract if there are no reservations in the remainder
        /// of the term of the specified contract.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static int Stop(Contract contract)
        {
            if (ReservationDB.ExistsReservationOfContract(contract))
            {
                throw new InvalidContractException(
                    "Er bestaan nog reservaties voor het einde van het huidige contract"
                );
            }

            if (contract.Formula == null)
            {
                contract = GetById(contract.Id);
            }

            contract.EndDate = DateTime.Now.Date.AddMonths(
                contract.Formula.NoticePeriodInMonths);
            return Save(contract);
        }

        private static bool ExistsConflictingContract(
            Contract contract, SqlConnection connection)
        {
            const string contractExistsQuery =
                @"SELECT CAsT(
                    CASE WHEN ( EXISTS(
                      SELECT ct.* FROM Contract ct
                      WHERE ct.Id != @Id
                        AND ct.CompanyId = @CompanyId
                        AND ct.StartDate BETWEEN @StartDate AND @EndDate
                        AND ct.EndDate BETWEEN @StartDate AND @EndDate
                    )) THEN 1 ELSE 0 END
                  AS BIT)";
            return connection.Query<bool>(
                contractExistsQuery, contract
            ).Single();
        }

        private static bool isNew(Contract contract)
        {
            return contract.Id == 0;
        }

        /// <summary>
        /// Delete the specified contract.
        /// </summary>
        /// <param name="contract">The contract to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(Contract contract)
        {
            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Execute(sql: deleteQuery, param: contract);
            }
        }

        /// <summary>
        /// Delete the specified contracts.
        /// </summary>
        /// <param name="contracts">The companies to be deleted.</param>
        /// <returns>Number of affected rows.</returns>
        public static int Delete(IEnumerable<Contract> contracts)
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
                            param: contracts,
                            transaction: transaction
                        );
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + exception,
                                                           "ContractDB Delete");
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Get the contract to which the specified reservation belongs.
        /// </summary>
        /// <param name="reservation">The reservation of which the contract 
        /// is requested</param>
        /// <returns>The contract of the specified reservation</returns>
        internal static Contract GetContractForReservation(Reservation reservation)
        {
            string selectContractExists = 
                selectAllQuery + 
                @"WHERE ct.CompanyId = @CompanyId
                    AND @StartDate BETWEEN ct.StartDate AND ct.EndDate
                    AND @EndDate BETWEEN ct.StartDate And ct.EndDate";

            using (SqlConnection connection = SamenSterkerDB.GetConnection())
            {
                return connection.Query<Contract, Company, ContractFormula, Contract>(
                   selectContractExists, Mapper, reservation
               ).SingleOrDefault();
            }
        }

    }
}
