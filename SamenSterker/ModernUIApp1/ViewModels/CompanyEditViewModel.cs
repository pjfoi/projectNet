using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class CompanyEditViewModel : BaseViewModel
    {
        #region Properties
        private Company company;
        public Company Company
        {
            get { return company; }
            set 
            { 
                company = value;
                OnPropertyChanged("Company");
            }
        }

        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public CompanyEditViewModel()
        {
            ShowCompany();
            CreateCommands();
        }

        private void CreateCommands()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                int nbFieldsLeftOpen = NbRequiredFieldsOpen(
                    Company.Name,
                    Company.Street,
                    Company.Zipcode,
                    Company.City,
                    Company.Country,
                    Company.Email,
                    Company.Phone,
                    Company.Employees
                );
                if (nbFieldsLeftOpen > 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(
                        String.Format("U hebt {0} veld(en) niet ingevuld.", nbFieldsLeftOpen),
                        "Misukt", System.Windows.MessageBoxButton.OK
                    );
                    return;
                }

                CompanyDB.Save(Company);

                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Bedrijf opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                );

                Mediator.NotifyColleagues<string>(MediatorMessages.CompanyEdit);

                INavigationService navigator = new NavigationService();
                navigator.Navigate<CompanyOverviewViewModel>();

            }//,
                //canExecute: (obj) => { return AreMultipleContractsSelected(); }
            );
        }

        public void ShowCompany()
        {
            ShowCompany(new Company());
        }

        public void ShowCompany(Company company)
        {
            Company = company;
            System.Diagnostics.Debug.WriteLine(
                String.Format("Show Company {0}", company),
                "CompanyEditVM"
            );
        }

    }
}
