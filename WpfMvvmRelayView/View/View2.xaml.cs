using System.Windows.Controls;
using System.Windows.Media;
using WpfMvvmRelayView.ViewModel;

namespace WpfMvvmRelayView.View
{
    public partial class View2 : UserControl
    {
        public View2()
        {
            InitializeComponent();
            var view2Vm = new View2VM();
            DataContext = view2Vm;
        }
    }

    
}