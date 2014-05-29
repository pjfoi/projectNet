using SamenSterkerData;
using System;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// CompanyEditViewModel : Create or edit a company
    /// </summary>
    public class CompanyEditViewModel : BaseViewModel
    {
        #region Properties
        private Company company;

        /// <summary>
        /// Company which is edited/created
        /// </summary>
        public Company Company
        {
            get { return company; }
            set 
            { 
                company = value;
                OnPropertyChanged("Company");
            }
        }

        /// <summary>
        /// Command to save the company
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new CompanyEditViewModel
        /// </summary>
        public CompanyEditViewModel()
        {
            CreateSaveCommand();
        }

        private void CreateSaveCommand()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                // check if all required fields have been filled in
                int nbFieldsLeftOpen = GetNbRequiredFieldsLeftOpen();
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen),
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                // save the company
                CompanyDB.Save(Company);
                Mediator.NotifyColleagues<string>(MediatorMessages.CompanyEdit, "");
                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Bedrijf opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                );

                // navigate to the company overview
                Navigator.Navigate<CompanyOverviewViewModel>();
            });
        }

        /// <summary>
        /// Show a default company in the edit form.
        /// </summary>
        public void ShowCompany()
        {
            ShowCompany(new Company());
        }

        /// <summary>
        /// Show the specified company in the edit form.
        /// </summary>
        /// <param name="company"></param>
        public void ShowCompany(Company company)
        {
            Company = company;
        }

        private int GetNbRequiredFieldsLeftOpen()
        {
            return NbRequiredFieldsOpen(
                Company.Name,
                Company.Street,
                Company.Zipcode,
                Company.City,
                Company.Country,
                Company.Email,
                Company.Phone,
                Company.Employees
            );
        }

    }
}
