using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfMvvmRelayView.Model
{
    public static class DataGridBehavior
    {
        public static readonly DependencyProperty SelectedIndicesProperty =
            DependencyProperty.RegisterAttached(
                "SelectedIndices",
                typeof(IList<int>),
                typeof(DataGridBehavior),
                new PropertyMetadata(null, OnSelectedIndicesChanged));

        public static IList<int> GetSelectedIndices(DependencyObject obj)
            => (IList<int>)obj.GetValue(SelectedIndicesProperty);

        public static void SetSelectedIndices(DependencyObject obj, IList<int> value)
            => obj.SetValue(SelectedIndicesProperty, value);

        private static void OnSelectedIndicesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if (e.OldValue is IList<int> oldList)
                {
                    dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
                }

                if (e.NewValue is IList<int> newList)
                {
                    dataGrid.SelectionChanged += DataGrid_SelectionChanged;
                }
            }
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            var selectedIndices = GetSelectedIndices(dataGrid);

            if (selectedIndices != null)
            {
                selectedIndices.Clear();
                foreach (var selectedItem in dataGrid.SelectedItems)
                {
                    int index = dataGrid.Items.IndexOf(selectedItem);
                    if (index >= 0)
                    {
                        selectedIndices.Add(index);
                    }
                }
            }
        }
    }
}