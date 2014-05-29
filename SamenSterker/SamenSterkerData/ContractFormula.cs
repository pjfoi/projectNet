using System;

namespace SamenSterkerData
{
    /// <summary>
    /// A Contract Formula Model.
    /// </summary>
    public class ContractFormula
    {
        private int id;
        private string description;
        private int maxUsageHoursPerPeriod;
        private int periodInMonths;
        private int noticePeriodInMonths;
        private float price;

        /// <summary>
        /// The id of the contract formula.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The description of the contract formula.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The maximum hours which can be used in the period of the 
        /// contract with the contract formula.
        /// </summary>
        public int MaxUsageHoursPerPeriod
        {
            get { return maxUsageHoursPerPeriod; }
            set { maxUsageHoursPerPeriod = value; }
        }

        /// <summary>
        /// The period of a contract with the contract formula.
        /// </summary>
        public int PeriodInMonths
        {
            get { return periodInMonths; }
            set { periodInMonths = value; }
        }

        /// <summary>
        /// The notice period of a contract with the contract formula.
        /// </summary>
        public int NoticePeriodInMonths
        {
            get { return noticePeriodInMonths; }
            set { noticePeriodInMonths = value; }
        }

        /// <summary>
        /// The price of a contract with the contract formula.
        /// </summary>
        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// Get a textual representation of the contract formula.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} (id {1})", Description, Id);
        }
    }
}
