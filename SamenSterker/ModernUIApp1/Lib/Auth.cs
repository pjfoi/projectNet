using MediatorLib;
using SamenSterkerData;
using System.ComponentModel;
using System.Web.Security;
using WebMatrix.WebData;

namespace UserInteface.Lib
{
    /// <summary>
    /// Authorization / Authentication class to login users,store
    /// the currently logged in user and store if the current user
    /// is an admin or a client.
    /// </summary>
    public class Auth : INotifyPropertyChanged
    {
        #region Properties
        private User _user;

        /// <summary>
        /// The currently logged in user
        /// </summary>
        public User User
        {
            get { return _user; }
            internal set
            {
                _user = value;
                UpdateCurrentUser();
                OnPropertyChanged("User");
            }
        }

        private bool _isAdmin;

        /// <summary>
        /// Is the currently logged in user an admin.
        /// </summary>
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

        /// <summary>
        /// Is the currently logged in user a client.
        /// </summary>
        public bool isClient
        {
            get { return _isClient; }
            internal set
            {
                _isClient = value;
                OnPropertyChanged("IsClient");
            }
        }

        private static readonly Mediator mediator = ((App)App.Current).Mediator;

        private Mediator Mediator
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

            /*if (!Roles.RoleExists("admin"))
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
                Roles.AddUsersToRole(new string[] { "involved", "cronos" }, "client");*/
        }

        /// <summary>
        /// Login the user with the specified username and password if they are correct.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Whether or not the username/password combination is correct.</returns>
        public bool Login(string username, string password)
        {
            bool succes = false;
            if (System.Web.Security.Membership.ValidateUser(username, password))
            {
                User = UserDB.GetByUsername(username);
                succes = true;
            }
            return succes;
        }

        /// <summary>
        /// Logout the currenly logged in user.
        /// </summary>
        public void Logout()
        {
            User = null;
            Mediator.NotifyColleagues<string>(MediatorMessages.Logout, "");
        }

        private void UpdateCurrentUser()
        {
            if (User == null)
            {
                IsAdmin = false;
                isClient = false;
            }
            else
            {
                UpdateCurrentUserLogin();
            }
        }

        private void UpdateCurrentUserLogin()
        {
            IsAdmin = Roles.IsUserInRole(User.Username, "admin");
            isClient = Roles.IsUserInRole(User.Username, "client");

            if (IsAdmin)
            {
                Mediator.NotifyColleagues<User>
                    (MediatorMessages.LoginAdmin, User);
                System.Diagnostics.Debug.WriteLine(
                    string.Format("User {0} logged in as admin", User.Username),
                    "Auth"
                );
            }

            if (isClient)
            {
                Mediator.NotifyColleagues<User>
                    (MediatorMessages.LoginClient, User);
                System.Diagnostics.Debug.WriteLine(
                    string.Format("User {0} logged in as client", User.Username),
                    "Auth"
                );
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
