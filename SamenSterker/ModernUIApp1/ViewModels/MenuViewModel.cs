using FirstFloor.ModernUI.Presentation;
using MediatorLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            Mediator.Register(this);

            MenuLinkGroups = new LinkGroupCollection();
            SetDefaultMenu();
        }
        public LinkGroupCollection MenuLinkGroups{ get; private set; }

        private void SetDefaultMenu()
        {
            this.MenuLinkGroups.Clear();
            AddHomeGroup();
            AddSettingsGroup();
        }

        [MediatorMessageSink(MediatorMessages.LoginClient, ParameterType = typeof(string))]
        private void SetClientMenu(string username)
        {
            this.MenuLinkGroups.Clear();
            AddHomeGroup();
            AddSettingsGroup();
            AddContractGroup();
            AddReservationGroup();
        }

        [MediatorMessageSink(MediatorMessages.LoginAdmin, ParameterType = typeof(string))]
        private void SetAdminMenu(string username)
        {
            System.Diagnostics.Debug.WriteLine("update admin menu", "MenuViewModel");
            this.MenuLinkGroups.Clear();
            AddHomeGroup();
            AddSettingsGroup();
            AddCompanyGroup();
            AddContractGroup();
            AddReservationGroup();
        }

        private void AddHomeGroup() 
        {
            LinkGroup homeGroup = new LinkGroup { DisplayName = "Welcome" };
            homeGroup.Links.Add(CreateLink("home", "../Pages/Home.xaml"));
            homeGroup.Links.Add(CreateLink("login", "../Pages/Login.xaml"));
            MenuLinkGroups.Add(homeGroup);
        }

        private void AddSettingsGroup()
        {
            LinkGroup settingsGroup = new LinkGroup
            {
                DisplayName = "instellingen",
                GroupName = "Settings"
            };
            settingsGroup.Links.Add(CreateLink("software", "../Pages/Settings.xaml"));
            MenuLinkGroups.Add(settingsGroup);
        }

        private void AddContractGroup()
        {
            LinkGroup contractGroup = new LinkGroup { DisplayName = "contracten" };
            contractGroup.Links.Add(CreateLink("overzicht", "../Pages/ContractOverview.xaml"));
            contractGroup.Links.Add(CreateLink("toevoegen", "../Pages/ContractEdit.xaml"));
            MenuLinkGroups.Add(contractGroup);
        }

        private void AddReservationGroup()
        {
            LinkGroup reservationGroup = new LinkGroup { DisplayName = "reservaties" };
            reservationGroup.Links.Add(CreateLink("overzicht", "../Pages/ReservationOverview.xaml"));
            reservationGroup.Links.Add(CreateLink("kalendar", "../Pages/ReservationCalendar.xaml"));
            reservationGroup.Links.Add(CreateLink("toevoegen", "../Pages/ReservationEdit.xaml"));
            MenuLinkGroups.Add(reservationGroup);
        }

        private void AddCompanyGroup()
        {
            LinkGroup companyGroup = new LinkGroup { DisplayName = "bedrijven" };
            companyGroup.Links.Add(CreateLink("overzicht", "../Pages/CompanyOverview.xaml"));
            companyGroup.Links.Add(CreateLink("toevoegen", "../Pages/CompanyEdit.xaml"));
            MenuLinkGroups.Add(companyGroup);
        }

        private Link CreateLink(String displayName, String source)
        {
            return new Link 
            {
                DisplayName = displayName,
                Source = new Uri(source, UriKind.Relative)
            };
        }

    }
}
