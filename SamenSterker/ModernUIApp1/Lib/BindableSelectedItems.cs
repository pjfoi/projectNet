﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace UserInteface.Lib
{
    // source http://stackoverflow.com/a/17569580
    public class BindableSelectedItems : Behavior<DataGrid>
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", 
                typeof(IList), typeof(BindableSelectedItems),
                new PropertyMetadata(default(IList), OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs args)
        {
            var grid = ((BindableSelectedItems)sender).AssociatedObject;
            if (grid == null) return;

            // Add logic to select items in grid
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = (DataGrid)sender;
            SelectedItems = grid.SelectedItems;
        }
    }
}
