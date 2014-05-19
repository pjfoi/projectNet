using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SamenSterkerData;

namespace UserInteface.Pages
{
    class CompanyOverviewViewModel
    {

        public CompanyOverviewViewModel()
        {
            this.companies = new ObservableCollection<Company>(CompanyDB.GetAll());
        }

        private ObservableCollection<Company> companies;

        public IEnumerable<Company> Companies
        {
            get
            {
                return companies;
            }
        }

    }
}
