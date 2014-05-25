using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    public class CompanyEditViewModel
    {
        #region Properties
        private Company company;
        public Company Company
        {
            get { return company; }
            set { company = value; }
        }

        public DelegateCommand SaveCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public CompanyEditViewModel()
        {
            Company = new Company();
            CreateCommands();

            System.Diagnostics.Debug.WriteLine("create companyeditviewmodel");
        }

        private void CreateCommands()
        {
            SaveCommand = new DelegateCommand(execute: (obj) =>
            {
                CompanyDB.Save(Company);

                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Bedrijf opgeslagen", "Succes", System.Windows.MessageBoxButton.OK
                );

                INavigationService navigator = new NavigationService();
                navigator.Navigate<CompanyOverviewViewModel>();

            }//,
                //canExecute: (obj) => { return AreMultipleContractsSelected(); }
            );
        }

        public void ShowCompany(Company company)
        {
            Company = company;
            System.Diagnostics.Debug.WriteLine("Show Company");
        }

    }
}
