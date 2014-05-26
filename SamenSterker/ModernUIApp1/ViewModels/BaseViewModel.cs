using MediatorLib;
using System.ComponentModel;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        
        private static readonly INavigationService navigator = new NavigationService();

        protected static INavigationService GetNavigator()
        {
            return navigator;
        }


        static readonly Mediator mediator = ((App)App.Current).Mediator;
        
        public Mediator Mediator
        {
            get { return mediator; }
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
