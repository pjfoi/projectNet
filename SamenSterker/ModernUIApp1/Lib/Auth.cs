using MediatorLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace UserInteface.Lib
{
    public class Auth : INotifyPropertyChanged
    {
        #region Properties
        private string _username;
        public string Username
        {
            get { return _username; }
            internal set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            internal set
            {
                _isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        private bool _isClient;
        public bool isClient
        {
            get { return _isClient; }
            internal set
            {
                _isClient = value;
                OnPropertyChanged("IsClient");
            }
        }

        static readonly Mediator mediator = ((App)App.Current).Mediator;

        public Mediator Mediator
        {
            get { return mediator; }
        }
        #endregion Properties


        public static void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("auth initialize");

            WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection(
                connectionStringName: "LocalSqlServer",
                userTableName: "Users",
                userIdColumn: "Id",
                userNameColumn: "Username",
                autoCreateTables: true
            );

            if (!Roles.RoleExists("admin"))
                Roles.CreateRole("admin");

            if (!Roles.RoleExists("client"))
                Roles.CreateRole("client");

            if (! Roles.IsUserInRole("testuser", "admin"))
            {
                Roles.AddUserToRole("testuser", "admin");
            }
        }

        public bool Login(String username, String password)
        {
            bool succes = false;
            if (System.Web.Security.Membership.ValidateUser(username, password))
            {
                Username = username;
                UpdateCurrentUser();
                succes = true;
            }
            return succes;
        }

        public void Logout()
        {
            Username = "";
            Mediator.NotifyColleagues<String>(MediatorMessages.Logout);
        }

        private void UpdateCurrentUser()
        {
            IsAdmin = Roles.IsUserInRole(Username, "admin");
            isClient = Roles.IsUserInRole(Username, "client");

            if (IsAdmin)
            {
                Mediator.NotifyColleagues<String>
                    (MediatorMessages.LoginAdmin, Username);
                System.Diagnostics.Debug.WriteLine("is admin - send mediator message");
            }

            if (isClient)
            {
                Mediator.NotifyColleagues<String>
                    (MediatorMessages.LoginClient, Username);
            }
            
            

        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged
    }
}
