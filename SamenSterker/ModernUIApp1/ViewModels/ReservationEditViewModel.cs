using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ReservationEditViewModel : BaseViewModel
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
            Locations = LocationDB.GetAll().ToList<Location>();
            CreateCommands();
            ShowReservation();
        }

        private void CreateCommands()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
                {
                    // TEMPORARLY !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    Reservation.CompanyId = 2;

                    int nbFieldsLeftOpen = NbRequiredFieldsOpen(
                        Reservation.StartDate,
                        Reservation.EndDate,
                        Reservation.LocationId,
                        Reservation.CompanyId
                    );
                    if (nbFieldsLeftOpen > 0)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(
                            String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen), 
                            "Misukt", System.Windows.MessageBoxButton.OK
                        );
                        return;
                    }

                    ReservationDB.Save(Reservation);
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Reservatie opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                    );

                    Mediator.NotifyColleagues<string>(MediatorMessages.ReservationEdit);

                    INavigationService navigator = new NavigationService();
                    navigator.Navigate<ReservationOverviewViewModel>();

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

        public void ShowReservation()
        {
            ShowReservation(CreateDefaultReservation());
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

    }
}
