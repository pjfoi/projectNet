using SamenSterkerData;

namespace UserInteface.Pages
{
    public class ReservationOverviewViewModel : BaseOverviewViewModel<Reservation>
    {
        public ReservationOverviewViewModel() : base(
            name: "reservatie",
            items: ReservationDB.GetFromCompany(CompanyDB.GetById(2)),
            deleteItems: (reservations) => ReservationDB.Delete(reservations),
            editItem: (reservation) =>
            {
                GetNavigator().Navigate<ReservationEditViewModel>(reservation);
            }
        ) { }

    }
}
