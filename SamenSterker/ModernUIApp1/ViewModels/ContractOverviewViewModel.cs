using MediatorLib;
using SamenSterkerData;
using System;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ContractOverviewViewModel : BaseOverviewViewModel<Contract>
    {
        public ContractOverviewViewModel() : base(
            name: "contract",
            getItems: () => ContractDB.GetAll(),
            deleteItems: (contracts) => ContractDB.Delete(contracts),
            editItem: (contract) =>
            {
                GetNavigator().Navigate<ContractEditViewModel>(contract);
            }
        )
        {
            Mediator.Register(this);

            #region StopCommand
            StopCommand = new DelegateCommand(execute: (obj) =>
                {
                    Contract contract = (Contract) SelectedItems[0];

                    if (ReservationDB.ExistsReservationOfContract(contract))
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(
                            "Er bestaan nog reservaties voor het einde van het huidige contract",
                            "Mislukt", System.Windows.MessageBoxButton.OK
                        );
                        return;
                    }

                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Contract gestopt", "Succes", System.Windows.MessageBoxButton.OK
                    );

                },
                canExecute: (obj) => {
                    return IsOneItemSelected() 
                        && CanContractBeStopped((Contract)SelectedItems[0]);
                }
            );

            Commands.Add(StopCommand);
            #endregion StopCommand

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

        public DelegateCommand StopCommand
        {
            get;
            internal set;
        }

        [MediatorMessageSink(MediatorMessages.LoginAdmin, ParameterType = typeof(User))]
        private void ShowAllContracts(User user)
        {
            GetItems = () => ContractDB.GetAll();
            Refresh();
        }

        [MediatorMessageSink(MediatorMessages.LoginClient, ParameterType = typeof(User))]
        private void ShowUserCompanyContracts(User user)
        {
            GetItems = () => ContractDB.GetFromCompany(user.Company);
            Refresh();
        }

        [MediatorMessageSink(MediatorMessages.ContractEdit, ParameterType = typeof(string))]
        private void UpdateContracts(string parameter)
        {
            Refresh();
        }

    }
}
