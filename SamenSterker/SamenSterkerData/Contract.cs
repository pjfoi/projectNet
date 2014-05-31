using System;
using System.ComponentModel.DataAnnotations;

namespace SamenSterkerData
{
    /// <summary>
    /// A Contract Model
    /// </summary>
    public class Contract : ModelValidation
    {
        private int id;
        private int number;
        private DateTime startDate;
        private DateTime endDate;
        private Company company;
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
        [Required]
        public int Number
        {
            get { return number; }
            set 
            {
                number = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The start date of the contract.
        /// </summary>
        [Required]
        public DateTime StartDate
        {
            get { return startDate; }
            set 
            {
                startDate = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The end date of the contract.
        /// </summary>
        [Required]
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The company of this contract.
        /// </summary>
        [Required]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The id of the company of this contract.
        /// </summary>
        public int CompanyId
        {
            get { return (Company == null) ? 0 : Company.Id; }
        }

        /// <summary>
        /// The contract formula of this contract.
        /// </summary>
        [Required]
        public ContractFormula Formula
        {
            get { return formula; }
            set
            {
                formula = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The id of the contract formula of this contract.
        /// </summary>
        public int ContractFormulaId
        {
            get { return (Formula == null) ? 0 : Formula.Id; }
        }

        /// <summary>
        /// Is the specified object equal to the contract.
        /// </summary>
        /// <param name="obj">Object to check for equality</param>
        /// <returns>Equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Contract))
                return false;
  
            return ((Contract)obj).Id == this.Id;
        }

        /// <summary>
        /// Get a hashcode for the contract.
        /// </summary>
        /// <returns>An hashcode</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
