using MaimaiConsulationCenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaimaiConsulationCenter.Common;
using static MaimaiConsulationCenter.Common.Interfaces;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// SongsView.xaml 的交互逻辑
    /// </summary>
    public partial class SongsView : UserControl, IDataLoadable
    {
        public SongsView()
        {
            InitializeComponent();
        }
        public async Task InitializeDataAsync()
        {
            await new SongsViewModel().GetSongsDataAsync();
            this.DataContext = GlobalValues.SongsModel;
        }
    }
}
