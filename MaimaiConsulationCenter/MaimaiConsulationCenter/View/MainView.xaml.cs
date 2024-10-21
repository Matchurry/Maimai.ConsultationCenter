using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.ViewModel;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

            var currentDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..","..","Assets", "VLC");
            libDirectory = new DirectoryInfo(currentDirectory);
            bud.SourceProvider.CreatePlayer(libDirectory);

            bud.SourceProvider.MediaPlayer.SetMedia(new FileInfo(bud_src), new String[] { "input-repeat=65535" });
            bud.SourceProvider.MediaPlayer.Play();

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
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
