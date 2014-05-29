using MediatorLib;
using SamenSterkerData;
using System;
using System.Collections.ObjectModel;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// ReservationCalendarViewModel : Show the reservation on one date.
    /// </summary>
    public class ReservationCalendarViewModel : BaseViewModel
    {
        #region Properties
        private DateTime? selectedDate;

        /// <summary>
        /// Date of which the reservations are shown.
        /// </summary>
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

        /// <summary>
        /// The reservations on the selected date
        /// </summary>
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

        /// <summary>
        /// Command to add a new reservation
        /// </summary>
        public DelegateCommand AddCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new ReservationCalendarViewModel
        /// </summary>
        public ReservationCalendarViewModel()
        {
            CreateAddCommand();
            ShowTodaysReservations();
            Mediator.Register(this);
        }

        private void ShowTodaysReservations()
        {
            SelectedDate = DateTime.Now;
        }

        private void CreateAddCommand()
        {
            AddCommand = new DelegateCommand(execute: (obj) =>
            {
                Reservation reservation = new Reservation();
                reservation.StartDate = SelectedDate.Value.AddHours(8);
                reservation.EndDate = SelectedDate.Value.AddHours(9);

                Navigator.Navigate<ReservationEditViewModel>(reservation);
            },
                canExecute: (obj) => SelectedDate.HasValue
            );
        }

        private void GetReservationsForSelectedDate()
        {
            if (!SelectedDate.HasValue)
                return;

            ReservationsOnDate = new ObservableCollection<Reservation>(
                ReservationDB.GetAllOnDate(SelectedDate.Value)
            );
            System.Diagnostics.Debug.WriteLine(
                String.Format("Found {0} reservations on {1}", 
                    ReservationsOnDate.Count, SelectedDate.Value),
                "ReservationCalendarVM"
            );
        }

        [MediatorMessageSink(MediatorMessages.ReservationEdit, ParameterType = typeof(string))]
        private void UpdateReservations(string parameter)
        {
            GetReservationsForSelectedDate();
        }

    }
}
