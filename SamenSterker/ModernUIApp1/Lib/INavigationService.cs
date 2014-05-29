
namespace UserInteface.Lib
{
    /// <summary>
    /// NavigationService interface to navigate between ViewModels
    /// </summary>
    public interface INavigationService
    {
        void Navigate<T>(object parameter = null);

        void GoBack();
    }
}
