using MaimaiConsulationCenter.ViewModel;
using System.Windows.Controls;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// FirstPageView.xaml 的交互逻辑
    /// </summary>
    public partial class FirstPageView : UserControl
    {
        public FirstPageView()
        {
            InitializeComponent();
            DataContext = new FirstPageViewModel();
        }
    }
}
