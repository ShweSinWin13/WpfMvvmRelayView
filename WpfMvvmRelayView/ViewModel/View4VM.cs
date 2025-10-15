using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfMvvmRelayView.Command;
using WpfMvvmRelayView.View;

namespace WpfMvvmRelayView.ViewModel
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        private TreeViewItemViewModel _parent;
        private string _code;
        private string _name;
        private bool _isEditing;
        private string _path;
        private ImageSource _imageIcon;
        private ObservableCollection<TreeViewItemViewModel> _children = new ObservableCollection<TreeViewItemViewModel>();

        private bool _isCodeFirstPath;

        public bool IsCodeFirstPath
        {
            get => _isCodeFirstPath;
            set
            {
                _isCodeFirstPath = value;
                onPropertyChanged();
            }
        }

        public TreeViewItemViewModel Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                onPropertyChanged();
            }
        }
        
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                IsCodeFirstPath = _code.Equals(@"\");
                onPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; onPropertyChanged(); }
        }

        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; onPropertyChanged(); }
        }
        
        public string Path
        {
            get { return _path; }
            set { _path = value; onPropertyChanged(); }
        }

        public ImageSource Icon
        {
            get { return _imageIcon; }
            set { _imageIcon = value; onPropertyChanged(); }
        }

        public void clear()
        {
            Children.Clear();
        }
        
        public bool HasChildren()
        {
            return Children != null && Children.Count > 0;
        }
        
        /*public void AddChild(TreeViewItemViewModel child)
        {
            _children.Add(child);
        }*/

        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get => _children;
            set { _children = value; onPropertyChanged(); }
        }
    }
    public class View4VM: ViewModelBase
    {
        private ObservableCollection<TreeViewItemViewModel> _treeItems =
            new ObservableCollection<TreeViewItemViewModel>();
        public ObservableCollection<TreeViewItemViewModel> TreeItems
        {
            get => _treeItems;
            set
            {
                _treeItems = value;
                onPropertyChanged();
            }
        }
        
        public ICommand SpaceShortcutTreeView { get; set; }
        
        public ICommand PressEnterShortcutTreeView { get; set; }
        
        public ICommand CtrlInsertShortcutTreeView { get; set; }
        
        public ICommand DeleteShortcutTreeView { get; set; }
        
        public ICommand AddCmd { get; set; }
        public ICommand EditCmd { get; set; }
        
        public ICommand DeleteCmd { get; set; }

        private void Populate()
        {
            var main =  new TreeViewItemViewModel
            {
                Code = @"\",
                Name = "ParentChildTree",
                Path = @"\",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/document.png"))
            };
            
            //1Richard has two children: Andrey11, Miya12, Claude13
            var richard = new TreeViewItemViewModel
            {
                Parent = main,
                Code = "1",
                Name = "Richard",
                Path = "1",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/parent.png"))
            };

            var andrey = new TreeViewItemViewModel
            {
                Parent = richard,
                Code = "1",
                Name = "Andrey",
                Path = "1/1",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            var miya = new TreeViewItemViewModel
            {
                Parent = richard,
                Code = "2",
                Name = "Miya",
                Path = "1/2",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            var claude = new TreeViewItemViewModel
            {
                Parent = richard,
                Code = "3",
                Name = "Claude",
                Path = "1/3",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            richard.Children.Add(andrey);
            richard.Children.Add(miya);
            richard.Children.Add(claude);
            
            //2Maria has four children: Aaron21, Johnny22, Emily23, Sara24
            var maria =  new TreeViewItemViewModel
            {
                Parent = main,
                Code = "2",
                Name = "Maria",
                Path = "2",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/parent.png"))
            };
            
            var aaron =  new TreeViewItemViewModel
            {
                Parent = maria,
                Code = "1",
                Name = "Aaron",
                Path = "2/1",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            var johnny =  new TreeViewItemViewModel
            {
                Parent = maria,
                Code = "2",
                Name = "Johnny",
                Path = "2/2",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            var emily =  new TreeViewItemViewModel
            {
                Parent = maria,
                Code = "3",
                Name = "Emily",
                Path = "2/3",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            var sara =  new TreeViewItemViewModel
            {
                Parent = maria,
                Code = "4",
                Name = "Sara",
                Path = "2/1",
                Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/child.png"))
            };
            
            maria.Children.Add(aaron);
            maria.Children.Add(johnny);
            maria.Children.Add(emily);
            maria.Children.Add(sara);
            
            main.Children.Add(richard);
            main.Children.Add(maria);
            
            TreeItems.Add(main);
        }
        
        public View4VM()
        {
            Populate();

            SpaceShortcutTreeView = new RelayCommand(OnPressingSpace, IsAccessible);
            PressEnterShortcutTreeView = new RelayCommand(OnPressingEnter, IsAccessible);
            CtrlInsertShortcutTreeView = new RelayCommand(OnPressingCtrlInsert, IsAccessible);
            DeleteShortcutTreeView = new RelayCommand(OnDeleting, IsAccessible);

            AddCmd = new RelayCommand(OnPressingCtrlInsert, IsAccessible);
            EditCmd = new RelayCommand(OnPressingSpace, IsAccessible);
            DeleteCmd = new RelayCommand(OnDeleting, IsAccessible);
        }

        private bool IsAccessible(object obj)
        {
            return true;
        }

        private void OnPressingSpace(object obj)
        {
            if (obj is TreeView treeView)
            {
                var selectedItem = getSelectedTreeViewItem(treeView);
                if (selectedItem == null) return;
                selectedItem.IsEditing = true;
                focusToTextBox(treeView, selectedItem);   
            }
        }

        private void OnPressingEnter(object obj)
        {
            if (obj is TreeView treeView)
            {
                var selectedItem = getSelectedTreeViewItem(treeView);
                if (selectedItem == null) return;
                selectedItem.IsEditing = false;
            }
        }

        private void OnPressingCtrlInsert(object obj)
        {
            if (TreeItems.Count == 0 && obj is TreeView tv)
            {
                var newChild =  new TreeViewItemViewModel
                {
                    IsEditing = true,
                    Code = @"\",
                    Name = "",
                    Path = @"\",
                    Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/document.png"))
                };
                TreeItems.Add(newChild);
                
                //var treeViewItem = GetTreeViewItem(tv, TreeItems);
                //if (treeViewItem != null)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        //Update UI here
                        var newTreeViewItem = GetTreeViewItem(tv, newChild);
                        if (newTreeViewItem != null)
                        {
                            newTreeViewItem.BringIntoView();
                            newTreeViewItem.IsSelected = true;
                        }
                    }));
                    
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        //Update UI here
                        focusToTextBox(tv, newChild);
                    }));
                }
            }
            
            if (obj is TreeView treeView)
            {
                var selectedItem = getSelectedTreeViewItem(treeView);
                if (selectedItem == null) return;

                var no = "1";
                var parentOrChild = selectedItem.Code.Equals(@"\") ? "parent" : "child";
                if (selectedItem.HasChildren()) no = (selectedItem.Children.Count+1).ToString();
                var newChild =  new TreeViewItemViewModel
                {
                    IsEditing = true,
                    Code = no,
                    Name = "",
                    Path = $"{selectedItem.Path}/{no}",
                    Icon = new BitmapImage(new Uri($"pack://application:,,,/Images/{parentOrChild}.png"))
                };
                
                selectedItem.Children.Add(newChild);
                
                var treeViewItem = GetTreeViewItem(treeView, selectedItem);
                if (treeViewItem != null)
                {
                    treeViewItem.IsExpanded = true;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        //Update UI here
                        var newTreeViewItem = GetTreeViewItem(treeViewItem, newChild);
                        if (newTreeViewItem != null)
                        {
                            newTreeViewItem.BringIntoView();
                            newTreeViewItem.IsSelected = true;
                        }
                    }));
                    
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        //Update UI here
                        focusToTextBox(treeViewItem, newChild);
                    }));
                }
            }
        }

        private void OnDeleting(object obj)
        {
            if (obj is TreeView treeView)
            {
                var selectedItem = getSelectedTreeViewItem(treeView);
                if (selectedItem == null) return;

                if (selectedItem.Parent == null)
                {
                    TreeItems.Clear();
                    return;
                }
                if (selectedItem.Parent is TreeViewItemViewModel tviVm)
                {
                    tviVm.Children.Remove(selectedItem);
                }
            }
        }
        
        public TreeViewItemViewModel getSelectedTreeViewItem(TreeView treeView)
        {
            return treeView.SelectedItem as TreeViewItemViewModel;   
        }
        
        public void focusToTextBox(ItemsControl parent, object item)
        {
            var treeViewItem = GetTreeViewItem(parent, item);
            if (treeViewItem != null)
            {
                var textBox = FindVisualChild<System.Windows.Controls.TextBox>(treeViewItem);
                if (textBox != null)
                {
                    textBox.Focus();
                    textBox.SelectAll(); // Optionally select all text for easy editing
                }
            }
        }
        
        public TreeViewItem GetTreeViewItem(ItemsControl parent, object item)
        {
            if (parent == null)
            {
                return null;
            }

            // Check if the parent itself is the item
            TreeViewItem treeViewItem = parent.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
            if (treeViewItem != null) return treeViewItem;

            // Recursively search for the item in the children
            foreach (object child in parent.Items)
            {
                ItemsControl childControl = parent.ItemContainerGenerator.ContainerFromItem(child) as ItemsControl;
                treeViewItem = GetTreeViewItem(childControl, item);
                if (treeViewItem != null) return treeViewItem;
            }

            return null;
        }
        
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                {
                    return tChild;
                }

                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}