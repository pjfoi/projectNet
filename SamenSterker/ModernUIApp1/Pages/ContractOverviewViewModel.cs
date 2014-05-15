using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SamenSterkerData;

namespace UserInteface.Pages
{
    class ContractOverviewViewModel
    {
        public List<Contract> Contracts
        {
            get
            {
                return ContractDB.GetAll();
            }
        }
    }
}
