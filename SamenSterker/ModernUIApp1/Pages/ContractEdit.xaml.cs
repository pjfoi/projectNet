using FirstFloor.ModernUI.Windows;
using SamenSterkerData;
using System.Windows.Controls;
using UserInteface.Lib;
using UserInteface.ViewModels;

namespace UserInteface.Pages
{
    /// <summary>
    /// Interaction logic for ContractEdit.xaml
    /// </summary>
    public partial class ContractEdit : UserControl, IContent
    {
        public ContractEdit()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) { }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) { }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            Contract contract = NavigationService.GetParameter<Contract>(e);
            ContractEditViewModel vm = (ContractEditViewModel) this.DataContext;
            if (contract != null)
            {
                vm.ShowContract(contract);
            }
            else
            {
                vm.ShowContract();
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) { }
    }
}
