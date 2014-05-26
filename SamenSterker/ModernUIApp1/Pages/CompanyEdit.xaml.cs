using FirstFloor.ModernUI.Windows;
using Newtonsoft.Json;
using SamenSterkerData;
using System;
using System.Windows.Controls;
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
            // cheating (need an absolute path)
            Uri source = new Uri(new Uri("http://example.com"), e.Source);
            // get contract from url 
            string jsonParam = System.Web.HttpUtility.ParseQueryString(source.Query).Get("param");

            if (jsonParam != null)
            {
                Company company = JsonConvert.DeserializeObject<Company>(jsonParam);

                // show the passed company for editting
                ((CompanyEditViewModel) this.DataContext).ShowCompany(company);
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) { }
    }
}
