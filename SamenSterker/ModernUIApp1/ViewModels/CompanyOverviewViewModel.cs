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
        #region Properties
        public DelegateCommand AddContractCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public CompanyOverviewViewModel() : base("reservatie") {
            Mediator.Register(this);
            CreateAddContractCommand();
        }

        private void CreateAddContractCommand()
        {
            AddContractCommand = new DelegateCommand(execute: (obj) =>
            {
                Contract contract = new Contract();
                contract.Company = GetFirstSelectedItem();
                contract.CompanyId = contract.Company.Id;

                Navigator.Navigate<ContractEditViewModel>(contract);
            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );

            Commands.Add(AddContractCommand);
        }

        protected override void EditItem(Company company)
        {
            Navigator.Navigate<CompanyEditViewModel>(company);
        }

        protected override void DeleteItems(IEnumerable<Company> companies)
        {
            CompanyDB.Delete(companies);
        }

        protected override IEnumerable<Company> GetAdminItems(User user)
        {
            return CompanyDB.GetAll();
        }

        protected override IEnumerable<Company> GetClientItems(User user)
        {
            return Enumerable.Empty<Company>();
        }

        [MediatorMessageSink(MediatorMessages.ContractEdit, ParameterType = typeof(string))]
        private void UpdateCompanies(string parameter)
        {
            Refresh();
        }
    }
}
