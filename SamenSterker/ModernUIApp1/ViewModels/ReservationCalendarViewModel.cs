using MediatorLib;
using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ReservationCalendarViewModel : BaseViewModel
    {
        #region Properties
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

        public DelegateCommand AddCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public ReservationCalendarViewModel()
        {
            CreateCommands();
            Mediator.Register(this);

            // show reservations of today
            SelectedDate = DateTime.Now;
        }

        private void GetReservationsForSelectedDate()
        {
            if (!SelectedDate.HasValue)
                return;

            List<Reservation> reservations = ReservationDB.GetAllOnDate(SelectedDate.Value);
            ReservationsOnDate = new ObservableCollection<Reservation>(reservations);
            System.Diagnostics.Debug.WriteLine(
                String.Format("Found {0} reservations on {1}", reservations.Count, SelectedDate.Value),
                "ReservationCalendarVM"
            );
        }

        private void CreateCommands()
        {
            AddCommand = new DelegateCommand(execute: (obj) =>
            {
                Reservation reservation = new Reservation();
                reservation.StartDate = SelectedDate.Value.AddHours(8);
                reservation.EndDate = SelectedDate.Value.AddHours(9);

                Navigator.Navigate<ReservationEditViewModel>(reservation);
            },
                canExecute: (obj) => { return SelectedDate.HasValue; }
            );
        }

        [MediatorMessageSink(MediatorMessages.ReservationEdit, ParameterType = typeof(string))]
        private void UpdateReservations(string parameter)
        {
            GetReservationsForSelectedDate();
        }

    }
}
