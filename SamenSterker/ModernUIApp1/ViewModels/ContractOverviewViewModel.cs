using MediatorLib;
using SamenSterkerData;
using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ContractOverviewViewModel : BaseOverviewViewModel<Contract>
    {
        #region Properties
        public DelegateCommand StopCommand
        {
            get;
            internal set;
        }
        #endregion Properties

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
            canExecute: (obj) =>
                {
                    return IsOneItemSelected()
                        && CanContractBeStopped((Contract)SelectedItems[0]);
                }
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

        protected override void EditItem(Contract contract)
        {
            Navigator.Navigate<ContractEditViewModel>(contract);
        }

        protected override void DeleteItems(IEnumerable<Contract> contracts)
        {
            ContractDB.Delete(contracts);
        }

        protected override IEnumerable<Contract> GetAdminItems(User user)
        {
            return ContractDB.GetAll();
        }

        protected override IEnumerable<Contract> GetClientItems(User user)
        {
            return ContractDB.GetFromCompany(user.Company);
        }

        [MediatorMessageSink(MediatorMessages.ContractEdit, ParameterType = typeof(string))]
        private void UpdateContracts(string parameter)
        {
            Refresh();
        }

    }
}
