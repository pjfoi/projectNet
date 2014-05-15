using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class ContractFormula
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private int maxUsageHoursPerPeriod;

        public int MaxUsageHoursPerPeriod
        {
            get { return maxUsageHoursPerPeriod; }
            set { maxUsageHoursPerPeriod = value; }
        }
        private int periodInMonths;

        public int PeriodInMonths
        {
            get { return periodInMonths; }
            set { periodInMonths = value; }
        }
        private int noticePeriodInMonths;

        public int NoticePeriodInMonths
        {
            get { return noticePeriodInMonths; }
            set { noticePeriodInMonths = value; }
        }
        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public ContractFormula()
        {
        }

        public override string ToString()
        {
            return String.Format("{0} (id {1})", Description, Id);
        }
    }
}
