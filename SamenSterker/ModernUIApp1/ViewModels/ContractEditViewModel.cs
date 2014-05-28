using SamenSterkerData;
using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ContractEditViewModel : BaseViewModel
    {
        #region Properties
        private Contract contract;
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
        public IList<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        private IList<ContractFormula> formulas;
        public IList<ContractFormula> Formulas
        {
            get { return formulas; }
            set { formulas = value; }
        }

        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public ContractEditViewModel()
        {
            CreateCommands();
            Companies = CompanyDB.GetAll();
            Formulas = ContractFormulaDB.GetAll().ToList<ContractFormula>();
            ShowContract();
        }

        private Contract CreateDefaultContract()
        {
            Contract defaultContract = new Contract();
            defaultContract.StartDate = DateTime.Now;
            return defaultContract;
        }

        private void CreateCommands()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                // set company id if a client is logged in
                Auth auth = ((App)App.Current).Auth;
                if (auth.isClient)
                {
                    Contract.CompanyId = auth.User.CompanyId;
                }

                // check if all required fields have been filled in
                int nbFieldsLeftOpen = NbRequiredFieldsOpen(
                    Contract.CompanyId,
                    Contract.ContractFormulaId,
                    Contract.StartDate
                );
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen),
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                // sett end date based on selected contract formula
                Contract.Formula = GetContractFormulaByIdLocal(Contract.ContractFormulaId);
                Contract.EndDate = Contract.StartDate.AddMonths(Contract.Formula.PeriodInMonths);

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

        private ContractFormula GetContractFormulaByIdLocal(int id)
        {
            return Formulas.FirstOrDefault((f) => f.Id == id);
        }

        public void ShowContract()
        {
            ShowContract(CreateDefaultContract());
        }

        public void ShowContract(Contract contract)
        {
            Contract = contract;
        }
    }
}
