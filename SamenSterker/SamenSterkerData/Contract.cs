using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    class Contract
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        private int companyId;

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        private int contractFormulaId;

        public int ContractFormulaId
        {
            get { return contractFormulaId; }
            set { contractFormulaId = value; }
        }

        public Contract()
        {
        }
    }
}
