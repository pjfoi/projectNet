using MediatorLib;
using System.Windows;

namespace UserInteface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initialize the application
        /// </summary>
        public App()
        {
            UserInteface.Lib.Auth.Initialize();
            Auth = new UserInteface.Lib.Auth();
        }

        /// <summary>
        /// Auth object of this application
        /// </summary>
        public UserInteface.Lib.Auth Auth
        {
            get;
            internal set;
        }

        private static readonly Mediator mediator = new Mediator();

        /// <summary>
        /// Mediator object of this application
        /// </summary>
        public Mediator Mediator
        {
            get { return mediator;  }
        }

    }
}
