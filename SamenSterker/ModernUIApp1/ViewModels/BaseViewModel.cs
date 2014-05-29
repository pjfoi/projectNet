using MediatorLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    /// <summary>
    /// Abstract base class for View Models
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        
        private static readonly INavigationService navigator = new NavigationService();

        /// <summary>
        /// A navigator to navigate to the views of view models.
        /// </summary>
        protected INavigationService Navigator
        {
            get { return navigator;  }
        }

        /// <summary>
        /// Get a navigator object.
        /// </summary>
        /// <returns>a navigator</returns>
        protected static INavigationService GetNavigator()
        {
            return navigator;
        }

        static readonly Mediator mediator = ((App)App.Current).Mediator;
        
        /// <summary>
        /// A mediator to send or receive messages
        /// </summary>
        public Mediator Mediator
        {
            get { return mediator; }
        }
        
        // quick and dirty "required fields" check
        /// <summary>
        /// Get how many of the specified fiels are left open
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected int NbRequiredFieldsOpen(params object[] fields)
        {
            return fields.Count((item) => {
                return item == null || item.Equals(0) || String.IsNullOrWhiteSpace(item.ToString());
            });
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify that the property with the specified name has changed.
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged

    }
}
