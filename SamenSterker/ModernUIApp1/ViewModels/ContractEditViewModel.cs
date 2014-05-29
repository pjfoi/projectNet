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
                SetCompanyIdIfClient();
                SetEndDateBasedOnFormula();

                // check if all required fields have been filled in
                int nbFieldsLeftOpen = GetNbRequiredFieldsLeftOpen();
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen),
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

        private void SetCompanyIdIfClient()
        {
            Auth auth = ((App)App.Current).Auth;
            if (auth.isClient)
            {
                Contract.CompanyId = auth.User.CompanyId;
            }
        }

        private void SetEndDateBasedOnFormula()
        {
            SetContractFormula();
            Contract.EndDate = Contract.StartDate.AddMonths(
                Contract.Formula.PeriodInMonths
            );
        }

        private void SetContractFormula()
        {
            Contract.Formula = Formulas.FirstOrDefault(
                (f) => f.Id == Contract.ContractFormulaId
            );
        }

        private int GetNbRequiredFieldsLeftOpen()
        {
            return NbRequiredFieldsOpen(
                Contract.CompanyId,
                Contract.ContractFormulaId,
                Contract.StartDate
            );
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
