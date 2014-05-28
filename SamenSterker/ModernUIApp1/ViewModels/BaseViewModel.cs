using MediatorLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        
        private static readonly INavigationService navigator = new NavigationService();
        protected INavigationService Navigator
        {
            get { return navigator;  }
        }

        protected static INavigationService GetNavigator()
        {
            return navigator;
        }

        static readonly Mediator mediator = ((App)App.Current).Mediator;
        
        public Mediator Mediator
        {
            get { return mediator; }
        }


        // quick and dirty "required fields" check
        protected int NbRequiredFieldsOpen(params object[] fields)
        {
            return fields.Count((item) => item == null || item.Equals(0) || String.IsNullOrWhiteSpace(item.ToString()));
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
