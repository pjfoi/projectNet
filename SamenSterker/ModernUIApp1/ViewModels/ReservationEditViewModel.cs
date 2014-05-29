using MediatorLib;
using SamenSterkerData;
using SamenSterkerData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// ReservationEditViewModel : Create or edit a reservation.
    /// </summary>
    public class ReservationEditViewModel : BaseViewModel
    {

        #region Properties
        private Reservation reservation;

        /// <summary>
        /// The reservation which is created or edited.
        /// </summary>
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

        /// <summary>
        /// All the locations which can be selected.
        /// </summary>
        public IList<Location> Locations
        {
            get { return locations; }
            set 
            { 
                locations = value;
                OnPropertyChanged("Locations");
            }
        }

        private IList<Company> companies;

        /// <summary>
        /// All the companies which can reserve a location.
        /// </summary>
        public IList<Company> Companies
        {
            get { return companies; }
            set 
            { 
                companies = value;
                OnPropertyChanged("Companies");
            }
        }

        /// <summary>
        /// Command to save a reservation.
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new ReservationEditViewModel
        /// </summary>
        public ReservationEditViewModel()
        {
            UpdateLocations();
            UpdateCompanies();
            CreateSaveCommand();
            Mediator.Register(this);
        }

        private void UpdateCompanies()
        {
            Companies = CompanyDB.GetAll();
        }

        private void UpdateLocations()
        {
            Locations = LocationDB.GetAll().ToList<Location>();
        }

        private void CreateSaveCommand()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                SetCompanyIdIfClient();

                // check if all required fields have been filled in
                int nbFieldsLeftOpen = GetNbRequiredFieldsLeftOpen();
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen), 
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                try
                {
                    ReservationDB.IsReservationPossible(reservation);
                        
                    // save the reservation
                    ReservationDB.Save(Reservation);
                    Mediator.NotifyColleagues<string>(MediatorMessages.ReservationEdit, "");
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        "Reservatie opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                    );

                    // navigate to reservation overview
                    Navigator.Navigate<ReservationOverviewViewModel>();
                }
                catch (InvalidReservationException exception)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        exception.Message, "Mislukt", System.Windows.MessageBoxButton.OK
                    );
                }
                    
            });
        }

        private void SetCompanyIdIfClient()
        {
            Auth auth = ((App)App.Current).Auth;
            if (auth.isClient)
            {
                Reservation.CompanyId = auth.User.CompanyId;
            }
        }

        private int GetNbRequiredFieldsLeftOpen()
        {
            return NbRequiredFieldsOpen(
                Reservation.StartDate,
                Reservation.EndDate,
                Reservation.LocationId,
                Reservation.CompanyId
            );
        }

        /// <summary>
        /// Show a default reservation in the edit form.
        /// </summary>
        public void ShowReservation()
        {
            ShowReservation(CreateDefaultReservation());
        }

        /// <summary>
        /// Show the specified reservation in the edit form.
        /// </summary>
        public void ShowReservation(Reservation reservation)
        {
            Reservation = reservation;
        }

        private Reservation CreateDefaultReservation()
        {
            DateTime start = RoundMinutesUp(DateTime.Now, 5);
            return new Reservation { StartDate = start, EndDate = start.AddHours(1) };
        }

        private DateTime RoundMinutesUp(DateTime dt, int minutes)
        {
            return RoundUp(dt, TimeSpan.FromMinutes(minutes));
        }

        // source http://stackoverflow.com/a/7029464
        private DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        [MediatorMessageSink(MediatorMessages.CompanyEdit, ParameterType = typeof(string))]
        private void UpdateCompanies(string parameter)
        {
            UpdateCompanies();
        }

    }
}