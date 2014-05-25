using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    class ReservationListViewModel
    {
        #region Properties
        public ObservableCollection<Reservation> Reservations
        {
            get;
            internal set;
        }

        private IList<Object> selectedReservations;
        public IList<Object> SelectedReservations
        {
            get { return selectedReservations; }
            set {
                selectedReservations = value;
                // recheck if selected reservations can be edited / deleted
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand DeleteCommand
        {
            get;
            internal set;
        }

        public DelegateCommand EditCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public ReservationListViewModel()
        {
            Reservations = new ObservableCollection<Reservation>(
                ReservationDB.GetFromCompany(CompanyDB.GetById(2)) //ReservationDB.GetAll(); 
            );
            CreateCommands();
        }

        private void CreateCommands()
        {
            // delete the selected reservations
            DeleteCommand = new DelegateCommand(execute: (obj) => 
                {   
                     MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("Wilt u {0} reservatie(s) verwijderen?", SelectedReservations.Count),
                        "Bevestig Verwijdering", MessageBoxButton.YesNoCancel, MessageBoxImage.Question
                    );
                    
                    if (result == MessageBoxResult.Yes) 
                    {
                        IEnumerable<Reservation> deleteReservations = SelectedReservations.Cast<Reservation>();

                        // delete from DB
                        int rowsDeleted = ReservationDB.Delete(deleteReservations);

                        // delete from UI
                        // reverse list to make deleting possible
                        foreach (Reservation reservation in deleteReservations.Reverse())
	                    {
                            Reservations.Remove(reservation);
	                    }

                        System.Diagnostics.Debug.WriteLine(
                            String.Format("Deleted {0} reservations", rowsDeleted),
                            "ReservationListViewModel DeleteCommand"
                        );
                    }

                },
                canExecute: (obj) => { return AreMultipleReservationsSelected(); }
            );

            // navigate to the reservation edit form to edit the selected reservation
            EditCommand = new DelegateCommand(execute: (obj) =>
                {
                    Reservation reservation = (Reservation)SelectedReservations[0];

                    INavigationService navigator = new NavigationService();
                    navigator.Navigate<ReservationEditViewModel>(reservation);
                },
                canExecute: (obj) => { return IsOneReservationSelected(); }
            );

        }

        private bool IsOneReservationSelected()
        {
            return SelectedReservations != null && SelectedReservations.Count == 1;
        }

        private bool AreMultipleReservationsSelected()
        {
            return SelectedReservations != null && SelectedReservations.Count > 0;
        }

    }
}
