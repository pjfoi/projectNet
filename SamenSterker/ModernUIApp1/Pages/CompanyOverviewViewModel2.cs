using SamenSterkerData;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class CompanyOverviewViewModel2 : BaseOverviewViewModel<Company>
    {
        public CompanyOverviewViewModel2() : base(
            name: "reservatie",
            items: CompanyDB.GetAll(),
            deleteItems: (companies) => CompanyDB.Delete(companies),
            editItem: (company) => {
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
