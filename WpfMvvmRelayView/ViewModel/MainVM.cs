using System.Windows.Controls;
using System.Windows.Input;
using WpfMvvmRelayView.Command;
using WpfMvvmRelayView.View;

namespace WpfMvvmRelayView.ViewModel
{
    public class MainVM: ViewModelBase
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView; 
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    //onPropertyChanged(nameof(CurrentView));
                    onPropertyChanged();
                }
            }
        }

        public ICommand View1Command { get; set; }
        
        public ICommand View2Command { get; set; }
        
        public ICommand View3Command { get; set; }
        
        public ICommand View4Command { get; set; }

        public MainVM()
        {
            View1Command = new RelayCommand(ShowView1, CanShowView1);
            View2Command = new RelayCommand(ShowView2, CanShowView2);
            View3Command = new RelayCommand(ShowView3, CanShowView3);
            View4Command = new RelayCommand(ShowView4, CanShowView4);

            CurrentView = new View1();
        }

        private bool CanShowView4(object obj)
        {
            return true;
        }

        private void ShowView4(object obj)
        {
            CurrentView = new View4();
        }

        private bool CanShowView3(object obj)
        {
            return true;
        }

        private void ShowView3(object obj)
        {
            CurrentView = new View3();
        }

        private bool CanShowView2(object obj)
        {
            return true;
        }

        private void ShowView2(object obj)
        {
            CurrentView = new View2();
        }

        private void ShowView1(object obj)
        {
            CurrentView = new View1();
        }

        private bool CanShowView1(object obj)
        {
            return true;
        }
    }
}