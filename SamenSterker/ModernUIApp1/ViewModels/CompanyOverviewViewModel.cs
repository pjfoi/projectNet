using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SamenSterkerData;
using UserInteface.Lib;
using MediatorLib;

namespace UserInteface.ViewModels
{
    public class CompanyOverviewViewModel  : BaseOverviewViewModel<Company>
    {
        public CompanyOverviewViewModel() : base(
            name: "reservatie",
            getItems: () => CompanyDB.GetAll(),
            deleteItems: (companies) => CompanyDB.Delete(companies),
            editItem: (company) => {
                GetNavigator().Navigate<CompanyEditViewModel>(company);
            }
        ) {
            Mediator.Register(this);

            #region AddContractCommand
            AddContractCommand = new DelegateCommand(execute: (obj) =>
            {
                Contract contract = new Contract();
                contract.Company = (Company)SelectedItems[0];
                contract.CompanyId = contract.Company.Id;

                Navigator.Navigate<ContractEditViewModel>(contract);
            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );

            Commands.Add(AddContractCommand);
            #endregion AddContractCommand
        }

        public DelegateCommand AddContractCommand
        {
            get;
            internal set;
        }

        [MediatorMessageSink(MediatorMessages.LoginAdmin, ParameterType = typeof(User))]
        private void LoadCompanies(User user)
        {
            Refresh();
        }

        [MediatorMessageSink(MediatorMessages.ContractEdit, ParameterType = typeof(string))]
        private void UpdateCompanies(string parameter)
        {
            Refresh();
        }
    }
}
