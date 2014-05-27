using MediatorLib;
using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using WebMatrix.WebData;

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

        private User _user;
        public User User
        {
            get { return _user; }
            internal set
            {
                _user = value;
                OnPropertyChanged("User");
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

            WebSecurity.InitializeDatabaseConnection(
                connectionStringName: "LocalSqlServer",
                userTableName: "User",
                userIdColumn: "Id",
                userNameColumn: "UserName",
                autoCreateTables: true
            );

            if (!Roles.RoleExists("admin"))
                Roles.CreateRole("admin");
            
            if (!Roles.RoleExists("client"))
                Roles.CreateRole("client");

            if (!WebSecurity.UserExists("admin"))
                WebSecurity.CreateUserAndAccount("admin", "admin");

            if (!WebSecurity.UserExists("involved"))
                WebSecurity.CreateUserAndAccount("involved", "involved", new { CompanyId = 1 });

            if (!WebSecurity.UserExists("cronos"))
                WebSecurity.CreateUserAndAccount("cronos", "cronos", new { CompanyId = 2 });

            if (!Roles.IsUserInRole("admin", "admin"))
                Roles.AddUserToRole("admin", "admin");

            if (!Roles.IsUserInRole("involved", "client"))
                Roles.AddUsersToRole(new string[] { "involved", "cronos" }, "client");
        }

        public bool Login(String username, String password)
        {
            bool succes = false;
            if (System.Web.Security.Membership.ValidateUser(username, password))
            {
                UpdateCurrentUser(username);
                succes = true;
            }
            return succes;
        }

        public void Logout()
        {
            Username = "";
            Mediator.NotifyColleagues<String>(MediatorMessages.Logout);
        }

        private void UpdateCurrentUser(string username)
        {
            User = UserDB.GetByUsername(username);
            //System.Diagnostics.Debug.WriteLine(String.Format("username {0} company {1}", receivedUser.Username, receivedUser.Company.Name), "Auth");
            //this._user = receivedUser;
            Username = username;

            IsAdmin = Roles.IsUserInRole(Username, "admin");
            isClient = Roles.IsUserInRole(Username, "client");

            if (IsAdmin)
            {
                Mediator.NotifyColleagues<User>
                    (MediatorMessages.LoginAdmin, User);
                System.Diagnostics.Debug.WriteLine("is admin - send mediator message");
            }

            if (isClient)
            {
                Mediator.NotifyColleagues<User>
                    (MediatorMessages.LoginClient, User);
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
