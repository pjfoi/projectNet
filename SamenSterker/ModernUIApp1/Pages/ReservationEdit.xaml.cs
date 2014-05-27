using SamenSterkerData;
using System;
using System.Windows.Controls;
using UserInteface.ViewModels;

namespace UserInteface.Pages
{
    /// <summary>
    /// Interaction logic for ReservationEdit.xaml
    /// </summary>
    public partial class ReservationEdit : UserControl, FirstFloor.ModernUI.Windows.IContent
    {
        public ReservationEdit()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) { }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) { }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            ReservationEditViewModel vm = (ReservationEditViewModel) this.DataContext;
            // cheating (need an absolute path)
            Uri source = new Uri(new Uri("http://example.com"), e.Source);
            // get reservation from url 
            string jsonParam = System.Web.HttpUtility.ParseQueryString(source.Query).Get("param");

            if (jsonParam != null)
            {
                Reservation reservation = Newtonsoft.Json.JsonConvert.DeserializeObject<Reservation>(jsonParam);

                // show the passed reservation for editting
                vm.ShowReservation(reservation);
            }
            else
            {
                vm.ShowReservation();
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) { }

    }
}
