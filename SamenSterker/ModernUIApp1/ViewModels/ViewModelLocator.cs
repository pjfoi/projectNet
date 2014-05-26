using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInteface.ViewModels
{
    public class ViewModelLocator
    {
        #region Company ViewModels
        private CompanyEditViewModel companyEditVM = new CompanyEditViewModel();
        public CompanyEditViewModel CompanyEditViewModel
        {
            get { return companyEditVM; }
        }

        private CompanyOverviewViewModel companyOverviewVM = new CompanyOverviewViewModel();
        public CompanyOverviewViewModel CompanyOverviewViewModel
        {
            get { return companyOverviewVM; }
        }
        #endregion Company ViewModels

        #region Contract ViewModels
        private ContractEditViewModel contractEditVM = new ContractEditViewModel();
        public ContractEditViewModel ContractEditViewModel
        {
            get { return contractEditVM; }
        }

        private ContractOverviewViewModel contractOverviewVM = new ContractOverviewViewModel();
        public ContractOverviewViewModel ContractOverviewViewModel
        {
            get { return contractOverviewVM; }
        }
        #endregion Contract ViewModels
        
        #region Reservation ViewModels
        private ReservationEditViewModel reservationEditVM = new ReservationEditViewModel();
        public ReservationEditViewModel ReservationEditViewModel
        {
            get { return reservationEditVM; }
        }

        private ReservationOverviewViewModel reservationOverviewVM = new ReservationOverviewViewModel();
        public ReservationOverviewViewModel ReservationOverviewViewModel
        {
            get { return reservationOverviewVM; }
        }

        private ReservationCalendarViewModel reservationCalendarVM = new ReservationCalendarViewModel();
        public ReservationCalendarViewModel ReservationCalendarViewModel
        {
            get { return reservationCalendarVM; }
        }
        #endregion Reservation ViewModels

        #region Login ViewModel
        private LoginViewModel loginVM = new LoginViewModel();
        public LoginViewModel LoginViewModel
        {
            get { return loginVM; }
        }
        #endregion Login ViewModel

        #region Menu ViewModel
        private MenuViewModel menuVM = new MenuViewModel();
        public MenuViewModel MenuViewModel
        {
            get { return menuVM; }
        }
        #endregion Menu ViewModels
    }
}
