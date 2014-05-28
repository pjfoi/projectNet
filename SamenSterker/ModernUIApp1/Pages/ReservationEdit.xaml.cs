using SamenSterkerData;
using System.Windows.Controls;
using UserInteface.Lib;
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
            Reservation reservation = NavigationService.GetParameter<Reservation>(e);
            ReservationEditViewModel vm = (ReservationEditViewModel) this.DataContext;
            if (reservation != null)
            {
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
