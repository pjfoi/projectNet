
namespace UserInteface.ViewModels
{
    /// <summary>
    /// A class to initialize all the ViewModels.
    /// </summary>
    public class ViewModelLocator
    {
        #region Company ViewModels
        private CompanyEditViewModel companyEditVM = 
            new CompanyEditViewModel();
        
        /// <summary>
        /// A CompanyEditViewModel
        /// </summary>
        public CompanyEditViewModel CompanyEditViewModel
        {
            get { return companyEditVM; }
        }

        private CompanyOverviewViewModel companyOverviewVM = 
            new CompanyOverviewViewModel();

        /// <summary>
        /// A CompanyOverviewViewModel
        /// </summary>
        public CompanyOverviewViewModel CompanyOverviewViewModel
        {
            get { return companyOverviewVM; }
        }
        #endregion Company ViewModels

        #region Contract ViewModels
        private ContractEditViewModel contractEditVM = 
            new ContractEditViewModel();

        /// <summary>
        /// A ContractEditViewModel
        /// </summary>
        public ContractEditViewModel ContractEditViewModel
        {
            get { return contractEditVM; }
        }

        private ContractOverviewViewModel contractOverviewVM = 
            new ContractOverviewViewModel();

        /// <summary>
        /// A ContractOverviewViewModel
        /// </summary>
        public ContractOverviewViewModel ContractOverviewViewModel
        {
            get { return contractOverviewVM; }
        }
        #endregion Contract ViewModels
        
        #region Reservation ViewModels
        private ReservationEditViewModel reservationEditVM = 
            new ReservationEditViewModel();

        /// <summary>
        /// A ReservationEditViewModel
        /// </summary>
        public ReservationEditViewModel ReservationEditViewModel
        {
            get { return reservationEditVM; }
        }

        private ReservationOverviewViewModel reservationOverviewVM =
            new ReservationOverviewViewModel();

        /// <summary>
        /// A ReservationOverviewViewModel
        /// </summary>
        public ReservationOverviewViewModel ReservationOverviewViewModel
        {
            get { return reservationOverviewVM; }
        }

        private ReservationCalendarViewModel reservationCalendarVM = 
            new ReservationCalendarViewModel();

        /// <summary>
        /// A ReservationCalendarViewModel
        /// </summary>
        public ReservationCalendarViewModel ReservationCalendarViewModel
        {
            get { return reservationCalendarVM; }
        }
        #endregion Reservation ViewModels

        #region Login ViewModel
        private LoginViewModel loginVM = new LoginViewModel();

        /// <summary>
        /// A LoginViewModel
        /// </summary>
        public LoginViewModel LoginViewModel
        {
            get { return loginVM; }
        }
        #endregion Login ViewModel

        #region Menu ViewModel
        private MenuViewModel menuVM = new MenuViewModel();

        /// <summary>
        /// A MenuViewModel
        /// </summary>
        public MenuViewModel MenuViewModel
        {
            get { return menuVM; }
        }
        #endregion Menu ViewModels
    }
}
