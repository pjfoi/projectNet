using MediatorLib;
using SamenSterkerData;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class ReservationOverviewViewModel : BaseOverviewViewModel<Reservation>
    {
        public ReservationOverviewViewModel() : base(
            name: "reservatie",
            getItems: () => ReservationDB.GetAll(),//ReservationDB.GetFromCompany(CompanyDB.GetById(2)),
            deleteItems: (reservations) => ReservationDB.Delete(reservations),
            editItem: (reservation) =>
            {
                GetNavigator().Navigate<ReservationEditViewModel>(reservation);
            }
        ) {
            Mediator.Register(this);
        }

        [MediatorMessageSink(MediatorMessages.LoginAdmin, ParameterType = typeof(User))]
        private void ShowAllReservations(User user)
        {
            GetItems = () => ReservationDB.GetAll();
            Refresh();
        }

        [MediatorMessageSink(MediatorMessages.LoginClient, ParameterType = typeof(User))]
        private void ShowUserCompanyReservations(User user)
        {
            GetItems = () => ReservationDB.GetFromCompany(user.Company);
            Refresh();
        }

        [MediatorMessageSink(MediatorMessages.ReservationEdit, ParameterType = typeof(string))]
        private void UpdateReservations(string parameter)
        {
            Refresh();
        }
    }
}
