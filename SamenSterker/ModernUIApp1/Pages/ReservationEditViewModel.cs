using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class ReservationEditViewModel : INotifyPropertyChanged
    {

        #region Properties
        private Reservation reservation;
        public Reservation Reservation
        {
            get { return reservation; }
            set 
            { 
                reservation = value;
                OnPropertyChanged("Reservation");
            }
        }

        private IList<Location> locations;
        public IList<Location> Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public ReservationEditViewModel()
        {
            Reservation = CreateDefaultReservation();
            Locations = LocationDB.GetAll().ToList<Location>();
            CreateCommands();
        }

        private void CreateCommands()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
                {
                    System.Diagnostics.Debug.WriteLine("Save reservation");

                    // TEMPORARLY !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    Reservation.CompanyId = 2;

                    ReservationDB.Save(Reservation);
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Reservatie opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                    );

                    INavigationService navigator = new NavigationService();
                    navigator.Navigate<ReservationListViewModel>();

                    Reservation = CreateDefaultReservation();
                }//,
                //canExecute: (obj) => { return AreMultipleContractsSelected(); }
            );
        }

        private Reservation CreateDefaultReservation()
        {
            DateTime start = RoundUp(DateTime.Now, TimeSpan.FromMinutes(5));
            Reservation defaultReservation = new Reservation();
            defaultReservation.StartDate = start;
            defaultReservation.EndDate = start.AddHours(1);
            return defaultReservation;
        }

        public void ShowReservation(Reservation reservation)
        {
            Reservation = reservation;
        }

        // source http://stackoverflow.com/a/7029464
        private DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged


    }
}
