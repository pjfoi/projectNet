using MediatorLib;
using SamenSterkerData;
using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// ContractOverviewViewModel to show an overview of contract models
    /// </summary>
    public class ContractOverviewViewModel : BaseOverviewViewModel<Contract>
    {
        #region Properties
        /// <summary>
        /// Command to stop an ongoing contract.
        /// </summary>
        public DelegateCommand StopCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create an ContractOverviewViewModel
        /// </summary>
        public ContractOverviewViewModel() : base("contract")
        {
            Mediator.Register(this);
            CreateStopCommand();
        }

        private void CreateStopCommand()
        {
            StopCommand = new DelegateCommand(execute: (obj) =>
            {
                Contract contract = GetFirstSelectedItem();

                try
                {
                    ContractDB.Stop(contract);
                    Mediator.NotifyColleagues<string>(MediatorMessages.ContractEdit, "");
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Contract gestopt", "Succes", System.Windows.MessageBoxButton.OK
                    );
                }
                catch (InvalidContractException exception)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        exception.Message, "Mislukt", System.Windows.MessageBoxButton.OK
                    );
                }
            },
            canExecute: (obj) => IsOneItemSelected()
                        && CanContractBeStopped(GetFirstSelectedItem())
            );

            Commands.Add(StopCommand);
        }

        private bool CanContractBeStopped(Contract contract)
        {
            DateTime possibleNewEndDate = 
                DateTime.Now.Date.AddMonths(contract.Formula.NoticePeriodInMonths);
            // only ongoing contracts can be stopped
            // new enddate + noticeperiod must be before current end date
            return (contract.StartDate <= DateTime.Now.Date)
                && (possibleNewEndDate < contract.EndDate);
        }

        /// <summary>
        /// Edit the specified contract.
        /// </summary>
        /// <param name="contract">The contract to be edited.</param>
        protected override void EditItem(Contract contract)
        {
            Navigator.Navigate<ContractEditViewModel>(contract);
        }

        /// <summary>
        /// Delete the specified contracts.
        /// </summary>
        /// <param name="contracts">The contracts to be deleted.</param>
        protected override void DeleteItems(IEnumerable<Contract> contracts)
        {
            ContractDB.Delete(contracts);
        }

        /// <summary>
        /// Fetch the contracts for the overview for the specified admin user.
        /// </summary>
        /// <param name="user">The admin user.</param>
        /// <returns>The contracts for the specified admin user.</returns>
        protected override IEnumerable<Contract> FetchAdminItems(User admin)
        {
            return ContractDB.GetAll();
        }

        /// <summary>
        /// Fetch the contracts for the overview for the specified client user.
        /// </summary>
        /// <param name="user">The client user.</param>
        /// <returns>The contracts for the specified client user.</returns>
        protected override IEnumerable<Contract> FetchClientItems(User client)
        {
            return ContractDB.GetFromCompany(client.Company);
        }

        [MediatorMessageSink(MediatorMessages.ContractEdit, ParameterType = typeof(string))]
        private void UpdateContracts(string parameter)
        {
            Refresh();
        }

    }
}
