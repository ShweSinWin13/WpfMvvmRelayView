using WpfMvvmRelayView.View;

namespace WpfMvvmRelayView.ViewModel
{
    public class View1VM: ViewModelBase
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                onPropertyChanged();
            }
        }
        
        public View1VM()
        {
            Text = "View 1 (Initialized from VM)";
        }
    }
}