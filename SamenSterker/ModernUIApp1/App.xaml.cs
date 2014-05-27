using MediatorLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UserInteface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            //UserInteface.Lib.Auth.Initialize();

            //Auth = new UserInteface.Lib.Auth();
            //System.Diagnostics.Debug.WriteLine("Created Auth", "App");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            UserInteface.Lib.Auth.Initialize();

            Auth = new UserInteface.Lib.Auth();
            System.Diagnostics.Debug.WriteLine("Created Auth", "App");

            base.OnStartup(e);
        }


        public UserInteface.Lib.Auth Auth
        {
            get;
            internal set;
        }

        private static readonly Mediator mediator = new Mediator();

        public Mediator Mediator
        {
            get { return mediator;  }
        }

    }
}
