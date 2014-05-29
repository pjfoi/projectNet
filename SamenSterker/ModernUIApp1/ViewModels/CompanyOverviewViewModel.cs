using MediatorLib;
using SamenSterkerData;
using System.Collections.Generic;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// CompanyOverviewViewModel to show an overview of companies.
    /// </summary>
    public class CompanyOverviewViewModel  : BaseOverviewViewModel<Company>
    {
        #region Properties
        /// <summary>
        /// Command to add a contract for the selected company.
        /// </summary>
        public DelegateCommand AddContractCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new CompanyOverviewViewModel
        /// </summary>
        public CompanyOverviewViewModel() : base("bedrijf") {
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
                canExecute: (obj) => IsOneItemSelected()
            );

            Commands.Add(AddContractCommand);
        }

        /// <summary>
        /// Edit the specified company.
        /// </summary>
        /// <param name="company">The company to be deleted.</param>
        protected override void EditItem(Company company)
        {
            Navigator.Navigate<CompanyEditViewModel>(company);
        }

        /// <summary>
        /// Delete the specified companies.
        /// </summary>
        /// <param name="companies">The companies to be deleted.</param>
        protected override void DeleteItems(IEnumerable<Company> companies)
        {
            CompanyDB.Delete(companies);
        }

        /// <summary>
        /// Fetch the companies for the overview for the specified admin user.
        /// </summary>
        /// <param name="user">The admin user.</param>
        /// <returns>The companies for the overview for the admin user.</returns>
        protected override IEnumerable<Company> FetchAdminItems(User admin)
        {
            return CompanyDB.GetAll();
        }

        /// <summary>
        /// Fetch the companies for the overview for the specified client user.
        /// </summary>
        /// <param name="user">The client user.</param>
        /// <returns>The companies for the overview for the client user.</returns>
        protected override IEnumerable<Company> FetchClientItems(User client)
        {
            return Enumerable.Empty<Company>();
        }

        [MediatorMessageSink(MediatorMessages.CompanyEdit, ParameterType = typeof(string))]
        private void UpdateCompanies(string parameter)
        {
            Refresh();
        }
    }
}
