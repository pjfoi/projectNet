using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SamenSterkerData;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class CompanyOverviewViewModel  : BaseOverviewViewModel<Company>
    {
        public CompanyOverviewViewModel() : base(
            name: "reservatie",
            items: CompanyDB.GetAll(),
            deleteItems: (companies) => CompanyDB.Delete(companies),
            editItem: (company) => {
                System.Diagnostics.Debug.WriteLine(
                    "pass company " + company.ToString() + " for editting",
                    "CompanyOverviewVM"
                );
                GetNavigator().Navigate<CompanyEditViewModel>(company);
            }
        ) {

            AddContractCommand = new DelegateCommand(execute: (obj) =>
            {
                Contract contract = new Contract();
                contract.Company = (Company)SelectedItems[0];
                contract.CompanyId = contract.Company.Id;

                INavigationService navigator = new NavigationService();
                navigator.Navigate<ContractEditViewModel>(contract);
            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );

            Commands.Add(AddContractCommand);
        }

        public DelegateCommand AddContractCommand
        {
            get;
            internal set;
        }

    }
}
