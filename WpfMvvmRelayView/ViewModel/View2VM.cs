

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WpfMvvmRelayView.Command;
using WpfMvvmRelayView.View;

namespace WpfMvvmRelayView.ViewModel
{
    public class View2VM: ViewModelBase
    {
        private ObservableCollection<string> _data = new ObservableCollection<string>();
        public ObservableCollection<string> Data
        {
            get => _data;
            set
            {
                _data = value;
                onPropertyChanged();
            }
        }

        public string _selectedItem = null;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                onPropertyChanged();
            }
        }

        public int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                onPropertyChanged();
            }
        }

        public ICommand DeleteCmd { get; set; }
        public ICommand AddCmd { get; set; }

        private void Populate()
        {
            Data.Add("One");
            Data.Add("Two");
            Data.Add("Three");
            Data.Add("Four");
            Data.Add("Five");
            Data.Add("Six");
            Data.Add("Seven");
        }
        
        public View2VM()
        {
            Populate();

            DeleteCmd = new RelayCommand(OnDeleting, IsAccessible);
            AddCmd = new RelayCommand(OnAdding, IsAccessible);
        }

        private bool IsAccessible(object obj)
        {
            //Registry logic
            return true; //Default true for testing purpose only
        }

        private void OnDeleting(object obj)
        {
            //if (SelectedItem == null) return;
            //Data.Remove(SelectedItem);
            if (obj is ListView listView)
            {
                var indices = getSelectedIndicesAsSeenFromUI(listView);
                indices.Sort();
                indices.Reverse();

                foreach (var index in indices)
                {
                    Data.RemoveAt(index);
                }
            }
        }

        private void OnAdding(object obj)
        {
            if (obj is TextBox tb && !String.IsNullOrEmpty(tb.Text))
            {
                if (SelectedIndex != -1)
                {
                    Data.Insert(++SelectedIndex, tb.Text);
                    return;
                }
                Data.Add(tb.Text);   
            }
        }
        
        private List<int> getSelectedIndicesAsSeenFromUI(ListView listView)
        {
            if (listView.ItemsSource == null)
                return new List<int>();

            // Get the source collection
            var sourceCollection = listView.ItemsSource as IList;
            if (sourceCollection == null)
                return new List<int>();

            // Get sorted indices of selected rows
            List<int> sortedIndices = listView.SelectedItems
                .Cast<object>()
                .Select(selectedItem => sourceCollection.IndexOf(selectedItem))
                .Where(index => index >= 0) // Filter out -1 (not found)
                .ToList();

            return sortedIndices;
        }
    }
}
