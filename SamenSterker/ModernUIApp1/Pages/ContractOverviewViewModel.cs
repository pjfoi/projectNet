using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using SamenSterkerData;
using System.Windows.Input;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class ContractOverviewViewModel
    {

        public ContractOverviewViewModel()
        {
            this.contracts = new ObservableCollection<Contract>(ContractDB.GetAll());
            CreateCommands();
        }

        private ObservableCollection<Contract> contracts;
        public ObservableCollection<Contract> Contracts
        {
            get { return contracts; }
        }

        private IList<Object> _selectedContracts;
        public IList<Object> SelectedContracts
        {
            get { return _selectedContracts; }
            set { _selectedContracts = value; }
        }

        public ICommand DeleteCommand
        {
            get;
            internal set;
        }

        private void CreateCommands()
        {
            DeleteCommand = new DelegateCommand(execute: (obj) => {
                
                string deleteMsg = "Delete Contracts: ";
                // cast selecteditems to Contracts
                // reverse list to make deleting possible
                foreach (Contract contract in SelectedContracts.Cast<Contract>().Reverse())
	            {
                    deleteMsg += contract.Id + " " + contract.Company.Name + " - ";
                    Contracts.Remove(contract);
	            }
                
                System.Windows.MessageBox.Show(deleteMsg, "Delete Contracts");
            });
        }

    }
}
