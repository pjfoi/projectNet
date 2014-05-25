using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SamenSterkerData;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class CompanyOverviewViewModel
    {

        public CompanyOverviewViewModel()
        {
            this.companies = new ObservableCollection<Company>(CompanyDB.GetAll());
            CreateCommands();
        }

        private ObservableCollection<Company> companies;

        public ObservableCollection<Company> Companies
        {
            get { return companies; }
        }

        private IList<Object> _selectedCompanies;
        public IList<Object> SelectedCompanies
        {
            get { return _selectedCompanies; }
            set
            {
                _selectedCompanies = value;
                // recheck if selected companies can be edited / deleted
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                AddContractCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand DeleteCommand
        {
            get;
            internal set;
        }

        public DelegateCommand EditCommand
        {
            get;
            internal set;
        }

        public DelegateCommand AddContractCommand
        {
            get;
            internal set;
        }

        private void CreateCommands()
        {
            // delete the selected companies
            DeleteCommand = new DelegateCommand(execute: (obj) =>
            {
                // cast selecteditems to Companies
                // reverse list to make deleting possible
                foreach (Company company in SelectedCompanies.Cast<Company>().Reverse())
                {
                    Companies.Remove(company);

                    // TODO : DB deleting
                }
            },
                canExecute: (obj) => { return AreMultipleCompaniesSelected(); }
            );

            // navigate to the company edit form to edit the selected company
            EditCommand = new DelegateCommand(execute: (obj) =>
            {
                Company company = (Company)SelectedCompanies[0];

                INavigationService navigator = new NavigationService();
                navigator.Navigate<CompanyEditViewModel>(company);
            },
                canExecute: (obj) => { return IsOneCompanySelected(); }
            );

            // navigate to the contract edit form to edit the selected contract
            AddContractCommand = new DelegateCommand(execute: (obj) =>
            {
                Contract contract = new Contract();
                contract.Company = (Company)SelectedCompanies[0];
                contract.CompanyId = contract.Company.Id;

                INavigationService navigator = new NavigationService();
                navigator.Navigate<ContractEditViewModel>(contract);
            },
                canExecute: (obj) => { return IsOneCompanySelected(); }
            );
        }

        private bool IsOneCompanySelected()
        {
            return SelectedCompanies != null && SelectedCompanies.Count == 1;
        }

        private bool AreMultipleCompaniesSelected()
        {
            return SelectedCompanies != null && SelectedCompanies.Count > 0;
        }

    }
}
