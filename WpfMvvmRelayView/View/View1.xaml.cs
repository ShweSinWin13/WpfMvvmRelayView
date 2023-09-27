using System.Windows.Controls;
using WpfMvvmRelayView.ViewModel;

namespace WpfMvvmRelayView.View
{
    public partial class View1 : UserControl
    {
        public View1()
        {
            InitializeComponent();
            var view1Vm = new View1VM();
            DataContext = view1Vm;
        }
    }
}