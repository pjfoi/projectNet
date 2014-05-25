using SamenSterkerData;

namespace UserInteface.Pages
{
    class ReservationListViewModel2 : BaseOverviewViewModel<Reservation>
    {
        public ReservationListViewModel2() : base(
            name: "reservatie",
            items: ReservationDB.GetFromCompany(CompanyDB.GetById(2)),
            deleteItems: (reservations) => ReservationDB.Delete(reservations),
            editItem: (reservation) => {
                GetNavigator().Navigate<ReservationEditViewModel>(reservation);
            }
        ) { }

    }
}
