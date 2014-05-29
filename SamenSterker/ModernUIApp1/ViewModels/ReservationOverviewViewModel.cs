using MediatorLib;
using SamenSterkerData;
using System.Collections.Generic;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ReservationOverviewViewModel : BaseOverviewViewModel<Reservation>
    {
        public ReservationOverviewViewModel() : base("reservatie") 
        {
            Mediator.Register(this);
        }

        [MediatorMessageSink(MediatorMessages.ReservationEdit, ParameterType = typeof(string))]
        private void UpdateReservations(string parameter)
        {
            Refresh();
        }

        protected override void EditItem(Reservation reservation)
        {
            Navigator.Navigate<ReservationEditViewModel>(reservation);
        }

        protected override void DeleteItems(IEnumerable<Reservation> reservations)
        {
            ReservationDB.Delete(reservations);
        }

        protected override IEnumerable<Reservation> GetAdminItems(User user)
        {
            return ReservationDB.GetAll();
        }

        protected override IEnumerable<Reservation> GetClientItems(User user)
        {
            return ReservationDB.GetFromCompany(user.Company);
        }
    }
}
