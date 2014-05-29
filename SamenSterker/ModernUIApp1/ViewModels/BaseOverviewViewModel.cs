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
    /// <summary>
    /// Base class for an overview of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseOverviewViewModel<T> : BaseViewModel
    {
        #region Properties
        private ObservableCollection<T> items;

        /// <summary>
        /// Items of the overview
        /// </summary>
        public ObservableCollection<T> Items
        {
            get { return items; }
            private set {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private IList<Object> _selectedItems;

        /// <summary>
        /// Selected items in the overview
        /// </summary>
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

        /// <summary>
        /// Command to delete the selected items
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get;
            internal set;
        }

        /// <summary>
        /// Command to edit the selected item
        /// </summary>
        public DelegateCommand EditCommand
        {
            get;
            internal set;
        }

        private IList<DelegateCommand> commands;

        /// <summary>
        /// All the commands of the overview.
        /// The canExecute method is reevaluated after the selected items change.
        /// </summary>
        protected IList<DelegateCommand> Commands
        {
            get { return commands; }
            set { commands = value; }
        }

        private Func<User, IEnumerable<T>> getItems;

        /// <summary>
        /// Function to get the items.
        /// </summary>
        protected Func<User, IEnumerable<T>> GetItems
        {
            get { return getItems; }
            set { getItems = value; }
        }

        private string nameItems;
        #endregion Properties

        /// <summary>
        /// Create a BaseModelOverviewViewModel
        /// </summary>
        /// <param name="name">Name of the type of items.</param>
        public BaseOverviewViewModel(string name)
        {
            nameItems = name;
            Commands = new List<DelegateCommand>();
            CreateDeleteCommand();
            CreateEditCommand();

            Mediator.Register(MediatorMessages.LoginAdmin, (Action<User>)
                delegate(User user)
                {
                    GetItems = FetchAdminItems;
                    Refresh();
                });

            Mediator.Register(MediatorMessages.LoginClient, (Action<User>)
                delegate(User user) 
                {
                    GetItems = FetchClientItems;
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
                canExecute: (obj) => AreMultipleItemsSelected()
            );
            Commands.Add(DeleteCommand);
        }

        private void CreateEditCommand()
        {
            EditCommand = new DelegateCommand(execute: (obj) =>
            {
                EditItem(GetFirstSelectedItem());
            },
                canExecute: (obj) => IsOneItemSelected()
            );
            Commands.Add(EditCommand);
        }

        /// <summary>
        /// Get the first selected item.
        /// </summary>
        /// <returns>The first selected item.</returns>
        protected T GetFirstSelectedItem()
        {
            return (T)SelectedItems[0];
        }

        /// <summary>
        /// Get all the selected items.
        /// </summary>
        /// <returns>The selected items.</returns>
        protected IEnumerable<T> GetSelectedItems()
        {
            return SelectedItems.Cast<T>();
        }

        /// <summary>
        /// Refresh the items of the overview.
        /// </summary>
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

        /// <summary>
        /// Whether or not only one item is selected.
        /// </summary>
        /// <returns>Is one item selected.</returns>
        protected bool IsOneItemSelected()
        {
            return SelectedItems != null && SelectedItems.Count == 1;
        }

        /// <summary>
        /// Whether or not multiple items are selected.
        /// </summary>
        /// <returns>Are multiple items selected.</returns>
        protected bool AreMultipleItemsSelected()
        {
            return SelectedItems != null && SelectedItems.Count > 0;
        }

        /// <summary>
        /// Edit the specified item.
        /// </summary>
        /// <param name="item">The item to be edited.</param>
        abstract protected void EditItem(T item);

        /// <summary>
        /// Delete the specified items.
        /// </summary>
        /// <param name="items">The items to be deleted.</param>
        abstract protected void DeleteItems(IEnumerable<T> items);

        /// <summary>
        /// Fetch the items for the overview for the specified admin.
        /// </summary>
        /// <param name="user">The currently logged in user.</param>
        /// <returns>The items for the specified admin.</returns>
        abstract protected IEnumerable<T> FetchAdminItems(User admin);

        /// <summary>
        /// Fetch the items for the overview for the specified client.
        /// </summary>
        /// <param name="user">The client.</param>
        /// <returns>Tbe items for the specified client.</returns>
        abstract protected IEnumerable<T> FetchClientItems(User client);

    }
}
