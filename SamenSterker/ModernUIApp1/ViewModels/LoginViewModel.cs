using System;
using System.Web.Security;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class LoginViewModel
    {
        #region Properties
        private string username;
        public string Username
        {
            get { return username; }
            set 
            { 
                username = value;
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

                        System.Diagnostics.Debug.WriteLine(
                            String.Format("{0} is admin : {1} is client {2}",
                                          auth.Username,
                                          Roles.IsUserInRole(auth.Username, "admin"),
                                          Roles.IsUserInRole(auth.Username, "client")
                            ),
                            "App"
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
