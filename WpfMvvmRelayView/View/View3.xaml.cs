using System.Windows.Controls;
using WpfMvvmRelayView.ViewModel;

namespace WpfMvvmRelayView.View
{
    public partial class View3 : UserControl
    {
        public View3()
        {
            InitializeComponent();
            var view3Vm = new View3VM();
            DataContext = view3Vm;
        }
    }
}