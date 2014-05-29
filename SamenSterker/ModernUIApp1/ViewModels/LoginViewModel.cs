using System;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// LoginViewModel to handle logging in
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string username;

        /// <summary>
        /// Username of the user who wants to login.
        /// </summary>
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

        /// <summary>
        /// Command to log the user in.
        /// </summary>
        public DelegateCommand LoginCommand
        {
            get;
            internal set;
        }
        #endregion Properties

        /// <summary>
        /// Create a new LoginViewModel
        /// </summary>
        public LoginViewModel()
        {
            CreateLoginCommand();
        }

        private void CreateLoginCommand()
        {
            LoginCommand = new DelegateCommand(execute: (parameter) =>
                {
                    string password = (parameter as System.Windows.Controls.PasswordBox).Password;
                    Auth auth = ((App)App.Current).Auth;

                    if (auth.Login(Username, password))
                    {  
                        Xceed.Wpf.Toolkit.MessageBox.Show(
                            String.Format("Welkom {0}!", auth.User.Username),
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
                canExecute: (obj) => ! String.IsNullOrWhiteSpace(Username)
            );
        }

    }
}
