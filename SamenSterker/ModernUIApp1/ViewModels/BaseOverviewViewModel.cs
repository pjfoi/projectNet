using MediatorLib;
using SamenSterkerData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UserInteface.Lib;

namespace UserInteface.ViewModels
{
    public abstract class BaseOverviewViewModel<T> : BaseViewModel
    {
        #region Properties
        private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get { return items; }
            private set {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private IList<Object> _selectedItems;
        public IList<Object> SelectedItems
        {
            get { return _selectedItems; }
            set {
                _selectedItems = value;
                foreach(DelegateCommand cmd in Commands)
                {
                    cmd.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand DeleteCommand
        {
            get;
            internal set;
        }

        public DelegateCommand EditCommand
        {
            get;
            internal set;
        }

        private IList<DelegateCommand> commands;
        protected IList<DelegateCommand> Commands
        {
            get { return commands; }
            set { commands = value; }
        }

        private Func<User, IEnumerable<T>> getItems;
        protected Func<User, IEnumerable<T>> GetItems
        {
            get { return getItems; }
            set { getItems = value; }
        }

        private string nameItems;
        #endregion Properties

        public BaseOverviewViewModel(string name)
        {
            nameItems = name;
            Commands = new List<DelegateCommand>();
            CreateDeleteCommand();
            CreateEditCommand();

            Mediator.Register(MediatorMessages.LoginAdmin, (Action<User>)
                delegate(User user)
                {
                    GetItems = GetAdminItems;
                    Refresh();
                });

            Mediator.Register(MediatorMessages.LoginClient, (Action<User>)
                delegate(User user) 
                {
                    GetItems = GetClientItems;
                    Refresh();
                });
        }

        private void CreateDeleteCommand()
        {
            DeleteCommand = new DelegateCommand(execute: (obj) =>
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(
                   String.Format("Wilt u {0} {1}(s) verwijderen?", SelectedItems.Count, nameItems),
                   "Bevestig Verwijdering", MessageBoxButton.YesNoCancel, MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    IEnumerable<T> itemsToBeDeleted = GetSelectedItems();

                    // delete from DB
                    DeleteItems(itemsToBeDeleted);

                    // delete from UI
                    // reverse list to make deleting possible
                    foreach (T item in itemsToBeDeleted.Reverse())
                    {
                        Items.Remove(item);
                    }
                }
            },
                canExecute: (obj) => { return AreMultipleItemsSelected(); }
            );
            Commands.Add(DeleteCommand);
        }

        private void CreateEditCommand()
        {
            EditCommand = new DelegateCommand(execute: (obj) =>
            {
                EditItem(GetFirstSelectedItem());
            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );
            Commands.Add(EditCommand);
        }

        protected T GetFirstSelectedItem()
        {
            return (T)SelectedItems[0];
        }

        protected IEnumerable<T> GetSelectedItems()
        {
            return SelectedItems.Cast<T>();
        }

        public void Refresh()
        {
            System.Diagnostics.Debug.WriteLine(
                String.Format("Nb {0} before refresh {1}", nameItems, Items == null ? 0 : Items.Count),
                "BaseOverviewVM"
            );
            User user = ((App) App.Current).Auth.User;
            Items = new ObservableCollection<T>(GetItems(user));
            System.Diagnostics.Debug.WriteLine(
                String.Format("Nb {0} after refresh {1}", nameItems, Items.Count),
                "BaseOverviewVM"
            );
        }

        protected bool IsOneItemSelected()
        {
            return SelectedItems != null && SelectedItems.Count == 1;
        }

        protected bool AreMultipleItemsSelected()
        {
            return SelectedItems != null && SelectedItems.Count > 0;
        }

        abstract protected void EditItem(T item);

        abstract protected void DeleteItems(IEnumerable<T> items);

        abstract protected IEnumerable<T> GetAdminItems(User user);

        abstract protected IEnumerable<T> GetClientItems(User user);

    }
}
