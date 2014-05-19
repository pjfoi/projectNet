using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInteface.Pages
{
    class ContractEditViewModel
    {
        public ContractEditViewModel()
        {
            Companies = CompanyDB.GetAll().ToList<Company>();
            Formulas = ContractFormulaDB.GetAll().ToList<ContractFormula>();
        }

        private List<Company> companies;
        public List<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        private List<ContractFormula> formulas;
        public List<ContractFormula> Formulas
        {
            get { return formulas; }
            set { formulas = value; }
        }

    }
}
