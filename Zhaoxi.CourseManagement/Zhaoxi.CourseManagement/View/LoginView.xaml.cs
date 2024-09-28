using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;
using Zhaoxi.CourseManagement.ViewModel;

namespace Zhaoxi.CourseManagement.View
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public class LoginSuccessMessage { };

        public static readonly RoutedEvent AfterLogin = EventManager.RegisterRoutedEvent(
            "AfterLogin",RoutingStrategy.Bubble, typeof(RoutedEventHandler),typeof(Border));

        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            MainBoder.AddHandler(AfterLogin, new RoutedEventHandler(AfterLoginEvent));
            Messenger.Default.Register<LoginSuccessMessage>(this,OnLoginSuccess);

        }
        private void OnLoginSuccess(LoginSuccessMessage message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainBoder.RaiseEvent(new RoutedEventArgs(AfterLogin));//触发动画
            }));
        }

        private void WinMove_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void AfterLoginEvent(object sender, RoutedEventArgs e)
        {
            //MainBoder.RaiseEvent(new RoutedEventArgs(AfterLogin));
            return;
        }
    }
}
