using System.Windows;
using System.Windows.Controls;
using WpfMvvmRelayView.ViewModel;

namespace WpfMvvmRelayView.View
{
    public partial class View4 : UserControl
    {
        public View4()
        {
            InitializeComponent();
            var view4Vm = new View4VM();
            DataContext = view4Vm;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is TreeViewItemViewModel selectedItem)
            {
                selectedItem.IsEditing = false;
            }
            
        }
    }
}