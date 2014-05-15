using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SamenSterkerData;

namespace UserInteface.Pages
{
    class CompanyOverviewViewModel
    {
        public IEnumerable<Company> Companies
        {
            get
            {
                return CompanyDB.GetAll();
            }
        }

    }
}
