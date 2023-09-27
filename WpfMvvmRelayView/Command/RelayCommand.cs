using System;
using System.Windows.Input;

namespace WpfMvvmRelayView.Command
{
    public class RelayCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object> _Execute { get; set; }
        private Predicate<object> _canExecute { get; set; }

        public RelayCommand(Action<object> _ExecuteMethod, Predicate<object> _canExecuteMethod)
        {
            _Execute = _ExecuteMethod;
            _canExecute = _canExecuteMethod;
        }
        
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}