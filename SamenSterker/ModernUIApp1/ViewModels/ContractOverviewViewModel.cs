using SamenSterkerData;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ContractOverviewViewModel : BaseOverviewViewModel<Contract>
    {

        public ContractOverviewViewModel() : base(
            name: "contract",
            items: ContractDB.GetAll(),
            deleteItems: (contracts) => ContractDB.Delete(contracts),
            editItem: (contract) =>
            {
                GetNavigator().Navigate<ContractEditViewModel>(contract);
            }
        )
        {
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

    }
}
