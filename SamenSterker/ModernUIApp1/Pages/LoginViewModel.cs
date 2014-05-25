using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using UserInteface.Lib;
using WebMatrix.WebData;

namespace UserInteface.Pages
{
    class LoginViewModel
    {

        public LoginViewModel()
        {
            CreateCommands();

            WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection(
                connectionStringName: "LocalSqlServer",
                userTableName: "Users",
                userIdColumn: "Id",
                userNameColumn: "Username",
                autoCreateTables: true
            );

            //Roles.CreateRole("admin");

            if (! WebSecurity.UserExists("testuser"))
            {
                WebSecurity.CreateUserAndAccount("testuser", "Pass!");
            }

        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public DelegateCommand LoginCommand
        {
            get;
            internal set;
        }

        private void CreateCommands()
        {
            // delete the selected contracts
            LoginCommand = new DelegateCommand(execute: (parameter) =>
                {
                    string password = (parameter as System.Windows.Controls.PasswordBox).Password;
                    //if (Username.Equals("Test") && password.Equals("Test"))
                    if (Membership.ValidateUser(Username, password))
                    {
                        System.Windows.MessageBox.Show("Yiehaa", "Login");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Boehoe wrong! ", "No login");
                    }
                    Username = "";
                }/*,
                canExecute: (obj) =>
                {
                    return SelectedContracts != null && SelectedContracts.Count > 0;
                }*/
            );
        }


        /*if (Membership.FindUsersByName("testuser") == null)
        {
            Membership.CreateUser("testuser", "Pass!", "test@test.com", "Hood", "Pine Hills", true, out result);
            Console.WriteLine(result.ToString());
        }
        else
          Console.WriteLine("User " + "'testuser' bestaat in de DB!");

        if ( !Roles.RoleExists("Developer"))
            Roles.CreateRole("Developer");
        if (!Roles.IsUserInRole("testuser", "developer"))
          Roles.AddUserToRole("testuser", "Developer");

        if (Roles.IsUserInRole("testuser", "developer"))
            Console.WriteLine("Is a developer");
        else
            Console.WriteLine("Doesn't write code.");

        if (Membership.ValidateUser("testuser", "Pass!"))
            Console.WriteLine("User Validated.");
        else
            Console.WriteLine("User Invalid");*/

    }
}
