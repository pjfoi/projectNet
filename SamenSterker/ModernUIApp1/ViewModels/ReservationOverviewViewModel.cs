using MediatorLib;
using SamenSterkerData;
using System.Collections.Generic;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// ReservationOverviewViewModel to show an overview of reservations.
    /// </summary>
    public class ReservationOverviewViewModel : BaseOverviewViewModel<Reservation>
    {
        /// <summary>
        /// Create a new ReservationOverviewViewModel
        /// </summary>
        public ReservationOverviewViewModel() : base("reservatie") 
        {
            Mediator.Register(this);
        }

        /// <summary>
        /// Edit the the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation to be edited.</param>
        protected override void EditItem(Reservation reservation)
        {
            Navigator.Navigate<ReservationEditViewModel>(reservation);
        }

        /// <summary>
        /// Delete the specified reservations.
        /// </summary>
        /// <param name="reservations">The reservations to be deleted.</param>
        protected override void DeleteItems(IEnumerable<Reservation> reservations)
        {
            ReservationDB.Delete(reservations);
        }

        /// <summary>
        /// Fetch the reservations for the overview for the specified admin user.
        /// </summary>
        /// <param name="user">The admin user.</param>
        /// <returns>The reservations for the overview for the admin user.</returns>
        protected override IEnumerable<Reservation> FetchAdminItems(User admin)
        {
            return ReservationDB.GetAll();
        }

        /// <summary>
        /// Fetch the reservations for the overview for the specified client user.
        /// </summary>
        /// <param name="user">The client user.</param>
        /// <returns>The reservations for the overview for the client user.</returns>
        protected override IEnumerable<Reservation> FetchClientItems(User client)
        {
            return ReservationDB.GetFromCompany(client.Company);
        }

        [MediatorMessageSink(MediatorMessages.ReservationEdit, ParameterType = typeof(string))]
        private void UpdateReservations(string parameter)
        {
            Refresh();
        }
    }
}
