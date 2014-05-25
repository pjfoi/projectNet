using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UserInteface.Pages;


namespace UserInteface.Lib
{
    // source https://mui.codeplex.com/discussions/445631
    public class NavigationService : INavigationService
    {

        /// <summary>
        /// The view model routing.f
        /// </summary>
        private static readonly Dictionary<Type, string> viewModelRouting
            = new Dictionary<Type, string>
            {
                { typeof(CompanyOverviewViewModel), "/Pages/CompanyOverview.xaml" },
                { typeof(CompanyEditViewModel), "/Pages/CompanyEdit.xaml" },
                { typeof(ContractOverviewViewModel), "/Pages/ContractOverview.xaml" },
                { typeof(ContractEditViewModel), "/Pages/ContractEdit.xaml" },
                { typeof(ReservationCalendarViewModel), "/Pages/ReservationCalendar.xaml" },
                { typeof(ReservationOverviewViewModel), "/Pages/ReservationOverview.xaml" },
                { typeof(ReservationEditViewModel), "/Pages/ReservationEdit.xaml" },
                { typeof(LoginViewModel), "/Pages/Login.xaml" }
            };


        ModernFrame mainFrame;

        public NavigationService()
        {
            EnsureMainFrame();
        }

        private void EnsureMainFrame()
        {
            if (mainFrame == null)
            {
                var f = Application.Current.MainWindow;
                mainFrame = GetDescendantFromName(f, "ContentFrame") as ModernFrame;
            }
        }

        /// <summary>
        /// Gets the name of the descendant from.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <returns>Gets a descendant FrameworkElement based on its name. A descendant 
        /// FrameworkElement with the specified name or null if not found.</returns>
        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
                return null;

            FrameworkElement fe;

            for (int i = 0; i < count; i++)
            {
                fe = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (fe != null)
                {
                    if (fe.Name == name)
                        return fe;

                    fe = GetDescendantFromName(fe, name);
                    if (fe != null)
                        return fe;
                }
            }

            return null;
        }

        /// <summary>
        /// Navigates the specified parameter.
        /// </summary>
        /// <typeparam name="T">ViewModel type</typeparam>
        /// <param name="parameter">The parameter.</param>
        public void Navigate<T>(object parameter = null)
        {
            EnsureMainFrame();

            var navParameter = string.Empty;
            if (parameter != null)
            {
                navParameter = "?param=" + JsonConvert.SerializeObject(parameter);
            }

            if (viewModelRouting.ContainsKey(typeof(T)))
            {
                Uri newUrl = new Uri(viewModelRouting[typeof(T)] + navParameter, UriKind.Relative);
                Uri oldUrl = mainFrame.Source;
                mainFrame.Navigate(oldUrl, newUrl, NavigationType.New);
            }
        }

        /// <summary>
        /// Invokes the go back.
        /// </summary>
        public void GoBack()
        {
            NavigationCommands.BrowseBack.Execute(null, null);
        }
    }
}
