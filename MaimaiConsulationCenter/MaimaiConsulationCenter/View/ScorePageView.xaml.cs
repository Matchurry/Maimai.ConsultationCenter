using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using MaimaiConsulationCenter.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// PointsSearchView.xaml 的交互逻辑
    /// </summary>
    public partial class PointsSearchView : UserControl
    {
        public PointsSearchView()
        {
            InitializeComponent();
            InitializeDataAsync();
            //Console.WriteLine("loaded");
        }
        public void InitializeDataAsync()
        {
            var DataContext = new ScorePageViewModel().GetScorePageDataAsync();
            this.DataContext = DataContext;
        }

        private void Border_MouseEnterB35(object sender, MouseEventArgs e)
        {
            var item = (sender as Grid).DataContext;
            if (item is MaiUserScoresModel.Sd dxItem)
            {
                GlobalValues.B35_UI_Id = dxItem.id;
                Messenger.Default.Send(new B35MouseEnterMessage());
            }
        }

        private void Border_MouseEnterB15(object sender, MouseEventArgs e)
        {
            var item = (sender as Grid).DataContext;
            if (item is MaiUserScoresModel.Dx dxItem)
            {
                GlobalValues.B15_UI_Id = dxItem.id;
                Messenger.Default.Send(new B15MouseEnterMessage());
            }
        }

        private void Border_MouseLeaveB50(object sender, MouseEventArgs e)
        {
            Messenger.Default.Send(new B50MouseLeaveMessage());
        }
    }
}
