using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        public int ContractFormulaId
        {
            get { return contractFormulaId; }
            set { contractFormulaId = value; }
        }

        public Company Company
        {
            get { return company; }
            set { company = value; }
        }

        public ContractFormula Formula
        {
            get { return formula; }
            set { formula = value; }
        }

        public Contract()
        {
        }
    }
}
