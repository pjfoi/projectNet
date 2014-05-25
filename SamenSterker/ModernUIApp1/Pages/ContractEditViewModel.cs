using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class ContractEditViewModel : INotifyPropertyChanged
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
            Companies = CompanyDB.GetAll();
            Formulas = ContractFormulaDB.GetAll().ToList<ContractFormula>();
            Contract = CreateDefaultContract();

            CreateCommands();
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
                ContractDB.Save(Contract);
                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Contract opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                );

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

        public void ShowContract(Contract contract)
        {
            Contract = contract;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged
    }
}
