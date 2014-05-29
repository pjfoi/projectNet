using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    /// <summary>
    /// A Contract Model
    /// </summary>
    public class Contract
    {
        private int id;
        private int number;
        private DateTime startDate;
        private DateTime endDate;
        private int companyId;
        private Company company;
        private int contractFormulaId;
        private ContractFormula formula;

        /// <summary>
        /// The id of the contract.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        /// <summary>
        /// The number of the contract.
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// The start date of the contract.
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        /// <summary>
        /// The end date of the contract.
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        /// <summary>
        /// The id of the company of this contract.
        /// </summary>
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        /// <summary>
        /// The id of the contract formula of this contract.
        /// </summary>
        public int ContractFormulaId
        {
            get { return contractFormulaId; }
            set { contractFormulaId = value; }
        }

        /// <summary>
        /// The company of this contract.
        /// </summary>
        public Company Company
        {
            get { return company; }
            set { company = value; }
        }

        /// <summary>
        /// The contract formula of this contract.
        /// </summary>
        public ContractFormula Formula
        {
            get { return formula; }
            set { formula = value; }
        }

    }
}
