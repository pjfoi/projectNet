using MediatorLib;
using SamenSterkerData;
using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// ContractEditViewModel : Create or edit a contract.
    /// </summary>
    public class ContractEditViewModel : BaseViewModel
    {
        #region Properties
        private Contract contract;

        /// <summary>
        /// The contract which is edited or created.
        /// </summary>
        public Contract Contract
        {
            get { return contract; }
            set 
            {
                contract = value;
                OnPropertyChanged("Contract");
            }
        }

        private IList<Company> companies;

        /// <summary>
        /// All the companies
        /// </summary>
        public IList<Company> Companies
        {
            get { return companies; }
            set 
            { 
                companies = value;
                OnPropertyChanged("Companies");
            }
        }

        private IList<ContractFormula> formulas;

        /// <summary>
        /// All the contract formulas
        /// </summary>
        public IList<ContractFormula> Formulas
        {
            get { return formulas; }
            set 
            { 
                formulas = value;
                OnPropertyChanged("Formulas");
            }
        }

        /// <summary>
        /// Command to save the contract.
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new ContractEditViewModel
        /// </summary>
        public ContractEditViewModel()
        {
            UpdateCompanies();
            UpdateFormulas();
            CreateSaveCommand();
            Mediator.Register(this);
        }

        private void UpdateCompanies()
        {
            Companies = CompanyDB.GetAll();
        }

        private void UpdateFormulas()
        {
            Formulas = ContractFormulaDB.GetAll().ToList<ContractFormula>();
        }

        private void CreateSaveCommand()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                SetCompanyIfClient();
                SetEndDateBasedOnFormula();

                Contract.Validate();
                if (Contract.HasErrors)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "U hebt niet alle velden correct ingevuld.",
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                // save the contract
                try
                {
                    ContractDB.Save(Contract);
                    Mediator.NotifyColleagues<string>(MediatorMessages.ContractEdit, "");
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Contract opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                    );

                    // go to contract overview
                    Navigator.Navigate<ContractOverviewViewModel>();
                }
                catch (InvalidContractException exception)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        exception.Message, "Mislukt", System.Windows.MessageBoxButton.OK
                    );
                }
            });
        }

        private void SetCompanyIfClient()
        {
            Auth auth = ((App)App.Current).Auth;
            if (auth.isClient)
            {
                Contract.Company = auth.User.Company;
            }
        }

        private void SetEndDateBasedOnFormula()
        {
            //SetContractFormula();
            if (Contract.Formula != null)
            {
                Contract.EndDate = Contract.StartDate.AddMonths(
                    Contract.Formula.PeriodInMonths
                );
            }
        }

        /// <summary>
        /// Show a default contract in the edit form.
        /// </summary>
        public void ShowContract()
        {
            ShowContract(CreateDefaultContract());
        }

        /// <summary>
        /// Show the specified contract in the edit form.
        /// </summary>
        public void ShowContract(Contract contract)
        {
            Contract = contract;
        }

        private Contract CreateDefaultContract()
        {
            return new Contract() { StartDate = DateTime.Now }; ;
        }

        [MediatorMessageSink(MediatorMessages.CompanyEdit, ParameterType = typeof(string))]
        private void UpdateCompanies(string parameter)
        {
            UpdateCompanies();
        }

    }
}
