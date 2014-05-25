using System;

namespace UserInteface.Lib
{

    public interface INavigationService
    {
        void Navigate<T>(object parameter = null);

        void GoBack();
    }
}
