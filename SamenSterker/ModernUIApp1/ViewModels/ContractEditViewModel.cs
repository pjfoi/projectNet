using SamenSterkerData;
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
                int nbFieldsLeftOpen = NbRequiredFieldsOpen(
                    Contract.CompanyId,
                    Contract.ContractFormulaId,
                    Contract.StartDate,
                    Contract.EndDate
                );
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen),
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                ContractDB.Save(Contract);
                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Contract opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                );

                Mediator.NotifyColleagues<string>(MediatorMessages.ContractEdit);

                INavigationService navigator = new NavigationService();
                navigator.Navigate<ContractOverviewViewModel>();

                // TODO clear form
                Contract = CreateDefaultContract();
            }//,
                //canExecute: (obj) => { 
                //    return Contract.Formula != null;
                //}
            );
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
