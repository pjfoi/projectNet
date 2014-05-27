using MediatorLib;
using SamenSterkerData;
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
                Contract contract = (Contract)SelectedItems[0];

                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Contract gestopt", "Succes", System.Windows.MessageBoxButton.OK
                );

                // TODO DB stopping

            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );
            #endregion StopCommand

            Commands.Add(StopCommand);
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
