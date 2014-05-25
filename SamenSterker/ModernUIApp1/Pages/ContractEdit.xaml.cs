﻿using FirstFloor.ModernUI.Windows;
using Newtonsoft.Json;
using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInteface.Pages
{
    /// <summary>
    /// Interaction logic for ContractEdit.xaml
    /// </summary>
    public partial class ContractEdit : UserControl, IContent
    {
        public ContractEdit()
        {
            InitializeComponent();

            // create and assign the view model
            //this.viewmodel = new ContractEditViewModel();
            //this.DataContext = viewmodel;
        }

        //private ContractEditViewModel viewmodel;

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) { }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) { }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            // cheating (need an absolute path)
            Uri source = new Uri(new Uri("http://example.com"), e.Source);
            // get contract from url 
            string jsonParam = System.Web.HttpUtility.ParseQueryString(source.Query).Get("param");

            if (jsonParam != null)
            {
                Contract contract = JsonConvert.DeserializeObject<Contract>(jsonParam);

                // show the passed contract for editting
                ((ContractEditViewModel)this.DataContext).ShowContract(contract);
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) { }
    }
}
