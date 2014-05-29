using System;
using System.Web.Security;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string username;
        public string Username
        {
            get { return username; }
            set 
            { 
                username = value;
                OnPropertyChanged("Username");
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        public LoginViewModel()
        {
            CreateCommands();
        }

        private void CreateCommands()
        {
            LoginCommand = new DelegateCommand(execute: (parameter) =>
                {
                    string password = (parameter as System.Windows.Controls.PasswordBox).Password;
                    Auth auth = ((App)App.Current).Auth;

                    if (auth.Login(Username, password))
                    {  
                        Xceed.Wpf.Toolkit.MessageBox.Show(
                            String.Format("Welkom {0}!", auth.Username),
                            "Succes", System.Windows.MessageBoxButton.OK
                        );
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(
                            "Uw gebruikersnaam of wachtwoord is incorrect!",
                            "Mislukt", System.Windows.MessageBoxButton.OK
                        );
                    }
                    Username = "";
                },
                canExecute: (obj) =>
                {
                    return ! String.IsNullOrWhiteSpace(Username);
                }
            );
        }

    }
}
