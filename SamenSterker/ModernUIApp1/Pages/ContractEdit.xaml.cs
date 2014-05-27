using FirstFloor.ModernUI.Windows;
using Newtonsoft.Json;
using SamenSterkerData;
using System;
using System.Windows.Controls;
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
            ContractEditViewModel vm = ((ContractEditViewModel)this.DataContext);
            // cheating (need an absolute path)
            Uri source = new Uri(new Uri("http://example.com"), e.Source);
            // get contract from url 
            string jsonParam = System.Web.HttpUtility.ParseQueryString(source.Query).Get("param");

            if (jsonParam != null)
            {
                Contract contract = JsonConvert.DeserializeObject<Contract>(jsonParam);

                // show the passed contract for editting
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
