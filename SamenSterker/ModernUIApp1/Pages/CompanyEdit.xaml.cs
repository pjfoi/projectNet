using FirstFloor.ModernUI.Windows;
using SamenSterkerData;
using System.Windows.Controls;
using UserInteface.Lib;
using UserInteface.ViewModels;

namespace UserInteface.Pages
{
    /// <summary>
    /// Interaction logic for CompanyEdit.xaml
    /// </summary>
    public partial class CompanyEdit : UserControl, IContent
    {
        public CompanyEdit()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) { }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) { }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            Company company = NavigationService.GetParameter<Company>(e);
            CompanyEditViewModel vm = (CompanyEditViewModel) this.DataContext;
            if (company != null)
            {
                vm.ShowCompany(company);
            }
            else
            {
                vm.ShowCompany();
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) { }
    }
}
