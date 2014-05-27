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

        private Func<IEnumerable<T>> getItems;
        protected Func<IEnumerable<T>> GetItems
        {
            get { return getItems; }
            set { getItems = value; }
        }
        #endregion Properties

        public BaseOverviewViewModel(
            string name,
            Func<IEnumerable<T>> getItems,
            Action<IEnumerable<T>> deleteItems,
            Action<T> editItem
        )
        {
            //this.items = new ObservableCollection<T>(items);
            GetItems = getItems;

            #region DeleteCommand
            DeleteCommand = new DelegateCommand(execute: (obj) =>
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(
                   String.Format("Wilt u {0} {1}(s) verwijderen?", SelectedItems.Count, name),
                   "Bevestig Verwijdering", MessageBoxButton.YesNoCancel, MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    IEnumerable<T> itemsToBeDeleted = SelectedItems.Cast<T>();

                    // delete from DB
                    deleteItems(itemsToBeDeleted);

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
            #endregion DeleteCommand

            #region EditCommand
            EditCommand = new DelegateCommand(execute: (obj) =>
            {
                T item = (T)SelectedItems[0];
                editItem(item);
            },
                canExecute: (obj) => { return IsOneItemSelected(); }
            );
            #endregion EditCommand

            Commands = new List<DelegateCommand>();
            Commands.Add(DeleteCommand);
            Commands.Add(EditCommand);
        }

        public void Refresh()
        {
            Items = new ObservableCollection<T>(GetItems());
        }

        protected bool IsOneItemSelected()
        {
            return SelectedItems != null && SelectedItems.Count == 1;
        }

        protected bool AreMultipleItemsSelected()
        {
            return SelectedItems != null && SelectedItems.Count > 0;
        }
    }
}
