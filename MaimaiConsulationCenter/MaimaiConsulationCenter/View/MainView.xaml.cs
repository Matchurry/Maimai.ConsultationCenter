using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibVLCSharp.Shared;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.ViewModel;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private static DirectoryInfo libDirectory;
        private static string bud_src = "d:/MatchurryPanMoving/.NET/git/MaimaiConsultationCentre/MaimaiConsulationCenter/MaimaiConsulationCenter/Assets/Videos/bud.mp4";
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        public MainView()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;
            this.Topmost = true;
            MainViewModel model = new MainViewModel();
            DataContext = model;
            model.UserInfo.Avatar = GlobalValues.UserInfo.Avatar;
            model.UserInfo.UserName = GlobalValues.UserInfo.RealName;
            model.UserInfo.Gender = GlobalValues.UserInfo.Gender;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            var currentDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","Assets","VLC");
            libDirectory = new DirectoryInfo(currentDirectory);
            bud.SourceProvider.CreatePlayer(libDirectory);
            
            bud.SourceProvider.MediaPlayer.SetMedia(new FileInfo(bud_src), new String[]{"input-repeat=65535"});
            bud.SourceProvider.MediaPlayer.Play();

            //bud.SourceProvider.MediaPlayer.EndReached += MediaPlayerEndEvent;
        }

        private void MediaPlayerEndEvent(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            var obj = bud; //注意更改
            Task.Factory.StartNew(() =>
            {
                obj.Dispose();
                Application.Current.Dispatcher.BeginInvoke(new Action(()=>{
                    obj.SourceProvider.CreatePlayer(libDirectory);
                    obj.SourceProvider.MediaPlayer.Play(bud_src); //注意更改
                    obj.SourceProvider.MediaPlayer.EndReached += MediaPlayerEndEvent;
                }));
            });
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState==WindowState.Maximized?WindowState.Normal:WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
