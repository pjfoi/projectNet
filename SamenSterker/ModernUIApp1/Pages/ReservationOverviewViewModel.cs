using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SamenSterkerData;
using System.Collections.ObjectModel;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    public class ReservationOverviewViewModel : INotifyPropertyChanged
    {

        public ReservationOverviewViewModel()
        {
            CreateCommands();

            //ICollectionView view = CollectionViewSource.GetDefaultView(new List<Reservation>());
            //view.GroupDescriptions.Add(new PropertyGroupDescription("LocationId"));
            //view.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Ascending));
            //ReservationsOnDate = view;
            
            ReservationsOnDate = new ObservableCollection<Reservation>();
            SelectedDate = DateTime.Now;
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            { 
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
                GetReservationsForSelectedDate();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Reservation> reservationsOnDate;
        public ObservableCollection<Reservation> ReservationsOnDate
        {
            get { return reservationsOnDate; }
            set 
            {
                if (value != reservationsOnDate)
                {
                    reservationsOnDate = value;
                    OnPropertyChanged("ReservationsOnDate");
                }
            }
        }

        private void GetReservationsForSelectedDate()
        {
            if (!SelectedDate.HasValue)
                return;

            List<Reservation> reservations = ReservationDB.GetAllOnDate(SelectedDate.Value);

            System.Diagnostics.Debug.WriteLine("Found {0} reservations on {1}", reservations.Count, SelectedDate.Value);
            //System.Windows.MessageBox.Show("Test get reservations " + reservations.Count(), "test test");

            ReservationsOnDate = new ObservableCollection<Reservation>(reservations);
            //ReservationsOnDate = CollectionViewSource.GetDefaultView(reservations);
        }


        public DelegateCommand AddCommand
        {
            get;
            internal set;
        }

        private void CreateCommands()
        {
            AddCommand = new DelegateCommand(execute: (obj) =>
                {
                    System.Diagnostics.Debug.WriteLine("Add a new reservation on {0}", SelectedDate.Value);

                    Reservation reservation = new Reservation();
                    reservation.StartDate = SelectedDate.Value.AddHours(8);
                    reservation.EndDate = SelectedDate.Value.AddHours(9);

                    INavigationService navigator = new NavigationService();
                    navigator.Navigate<ReservationEditViewModel>(reservation);
                },
                canExecute: (obj) => { return SelectedDate.HasValue; }
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
