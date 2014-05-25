using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            set {
                _selectedContracts = value;
                // recheck if selected contracts can be edited / deleted
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand StopCommand
        {
            get;
            internal set;
        }

        private void CreateCommands()
        {
            // delete the selected contracts
            DeleteCommand = new DelegateCommand(execute: (obj) => 
                {
                    string deleteMsg = "Delete Contracts: ";
                    // cast selecteditems to Contracts
                    // reverse list to make deleting possible
                    foreach (Contract contract in SelectedContracts.Cast<Contract>().Reverse())
	                {
                        deleteMsg += contract.Id + " " + contract.Company.Name + " - ";
                        Contracts.Remove(contract);

                        // TODO : DB deleting
	                }
                
                    System.Windows.MessageBox.Show(deleteMsg, "Delete Contracts");
                },
                canExecute: (obj) => { return AreMultipleContractsSelected(); }
            );

            // navigate to the contract edit form to edit the selected contract
            EditCommand = new DelegateCommand(execute: (obj) =>
                {
                    Contract contract = (Contract) SelectedContracts[0];

                    INavigationService navigator = new NavigationService();
                    navigator.Navigate<ContractEditViewModel>(contract);
                },
                canExecute: (obj) => { return IsOneContractSelected(); }
            );

            StopCommand = new DelegateCommand(execute: (obj) =>
                {
                    Contract contract = (Contract)SelectedContracts[0];
                    System.Windows.MessageBox.Show(
                        String.Format("Stop Contract {0} of company {1}", contract.Number, contract.Company.Name),
                        "Stop Contract");

                    // TODO DB stopping

                },
                canExecute: (obj) => { return IsOneContractSelected(); }
            );
        }

        private bool IsOneContractSelected()
        {
            return SelectedContracts != null && SelectedContracts.Count == 1;
        }

        private bool AreMultipleContractsSelected()
        {
            return SelectedContracts != null && SelectedContracts.Count > 0;
        }

    }
}
