using System.ComponentModel;
using UserInteface.Lib;

namespace UserInteface.Pages
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        
        private static readonly INavigationService navigator = new NavigationService();

        protected static INavigationService GetNavigator()
        {
            return navigator;
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
