using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfMvvmRelayView.Command;
using WpfMvvmRelayView.View;

namespace WpfMvvmRelayView.ViewModel
{
    public class View3VM: ViewModelBase
    {
        private DataTable _dt = new DataTable();

        public DataTable Dt
        {
            get => _dt;
            set
            {
                _dt = value;
                onPropertyChanged();
            }
        }

        private bool _isChecked = true;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                onPropertyChanged();
            }
        }

        private int _selectedIndex=-1;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                onPropertyChanged();
            }
        }

        private string _no;
        private string _name;
        private string _age;

        public string No
        {
            get => _no;
            set
            {
                _no = value;
                onPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                onPropertyChanged();
            }
        }

        public string Age
        {
            get => _age;
            set
            {
                _age = value;
                onPropertyChanged();
            }
        }
        
        public ICommand CheckCmd { get; set; } 
        public ICommand AddCmd { get; set; }
        public ICommand DeleteCmd { get; set; }

        private void Populate()
        {
            _dt.Columns.Add("No", typeof(int));
            _dt.Columns.Add("Name", typeof(string));
            _dt.Columns.Add("Age", typeof(int));

            var row1 = _dt.NewRow();
            row1["No"] = 1;
            row1["Name"] = "Richard";
            row1["Age"] = 21;
            
            var row2 = _dt.NewRow();
            row2["No"] = 2;
            row2["Name"] = "Maria";
            row2["Age"] = 18;
            
            var row3 = _dt.NewRow();
            row3["No"] = 3;
            row3["Name"] = "Leonard";
            row3["Age"] = 25;

            _dt.Rows.Add(row1);
            _dt.Rows.Add(row2);
            _dt.Rows.Add(row3);
        }
        
        public View3VM()
        {
            Populate();

            CheckCmd = new RelayCommand(OnChecking, IsAccessible);
            AddCmd = new RelayCommand(OnAdding, IsAccessible);
            DeleteCmd = new RelayCommand(OnDeleting, IsAccessible);
        }

        private bool IsAccessible(object obj)
        {
            return true;
        }

        private void OnChecking(object obj)
        {
            
        }

        private void OnAdding(object obj)
        {
            if (String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Age))
            {
                MessageBox.Show("Please enter all fields!", "Stop!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var start = SelectedIndex + 1;
                
                var newRow = Dt.NewRow();
                newRow["No"] = SelectedIndex != -1 ? start : Dt.Rows.Count + 1;
                newRow["Name"] = Name;
                newRow["Age"] = Int32.Parse(Age);
                Dt.Rows.InsertAt(newRow, start);

                editRowNumbers(start);
            }
            catch
            {
                MessageBox.Show("No and Age must be integer!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnDeleting(object obj)
        {
            if (obj is DataGrid dataGrid)
            {
                var indices = getSelectedIndicesAsSeenFromUI(dataGrid);
                indices.Sort();
                indices.Reverse();
                
                foreach (var index in indices)
                {
                    Dt.Rows.RemoveAt(index);
                }
                
                editRowNumbers();
            }
        }

        private void editRowNumbers(int num=0)
        {
            for (int i = num; i < Dt.Rows.Count; i++)
            {
                Dt.Rows[i]["No"] = i+1;
            }
        }
        
        private List<int> getSelectedIndicesAsSeenFromUI(DataGrid dataGrid)
        {
            // Get the DataView (assuming your DataGrid is bound to a DataTable)
            DataView dataView = (DataView)dataGrid.ItemsSource;

            // Get sorted indices of selected rows
            List<int> sortedIndices = dataGrid.SelectedItems
                .Cast<DataRowView>()
                .Select(rowView => dataView.Table.Rows.IndexOf(rowView.Row))
                .ToList();

            return sortedIndices;
        }
        
       
    }
}