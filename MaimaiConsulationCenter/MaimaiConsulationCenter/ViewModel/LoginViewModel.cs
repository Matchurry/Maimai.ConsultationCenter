using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.DataAccess;
using MaimaiConsulationCenter.Model;
using MaimaiConsulationCenter.View;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MaimaiConsulationCenter.View.LoginView;

namespace MaimaiConsulationCenter.ViewModel
{

    public class LoginViewModel : NotifyBase
    {
        public static LoginModel LoginModel { get; set; } = new LoginModel("", "", "");
        public CommandBase CloseWindowCommand { get; set; }
        public CommandBase LoginCommand { get; set; }

        private string _errorMessage = "";
        private Visibility _showProgress = Visibility.Hidden;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; this.DoNotify(); }
        }

        public Visibility ShowProgress
        {
            get { return _showProgress; }
            set
            {
                _showProgress = value;
                this.DoNotify();
            }
        }

        public LoginViewModel()
        {
            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) =>
            {
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });
            this.LoginCommand = new CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin);
            this.LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

        }

        private void DoLoginButtonHidden(Button bt)
        {
            bt.IsEnabled = false;
            ControlTemplate template = bt.Template;
            Border bd = (Border)template.FindName("LoginButtonTemplateBorder", bt);
            bd.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DDD"));
        }

        private void DoLoginButtonRecover(Button bt)
        {
            bt.IsEnabled = true;
            ControlTemplate template = bt.Template;
            Border bd = (Border)template.FindName("LoginButtonTemplateBorder", bt);
            bd.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F79645"));
        }

        private TaskCompletionSource<bool> loginDataSource = new TaskCompletionSource<bool>(); //用以检测登录和数据计算是否结束
        private TaskCompletionSource<bool> dataLoadedSource = new TaskCompletionSource<bool>(); //用以检测缓存是否结束

        private void DoLogin(object o)
        {
            Button bt = (Button)(o as Window).FindName("LoginButton");
            DoLoginButtonHidden(bt);
            this.ShowProgress = Visibility.Visible;
            this.ErrorMessage = "";
            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                this.ErrorMessage = "请输入用户名！";
                this.ShowProgress = Visibility.Collapsed;
                DoLoginButtonRecover(bt);
                return;
            }

            Task.Run(new Action(async () =>
            {
                try
                {
                    loginDataSource = new TaskCompletionSource<bool>();
                    dataLoadedSource = new TaskCompletionSource<bool>();
                    await Task.Run(() =>
                    {
                        LocalDataAccess.GetInstance().CheckUserInfo(LoginModel.UserName, LoginModel.Password);
                        loginDataSource.SetResult(true);
                    });
                    await loginDataSource.Task; //等待登录和计算结束

                    if (GlobalValues.UserInfo.UserName == null)
                    {
                        this.ShowProgress = Visibility.Collapsed;
                        throw new Exception("用户模型为空 请调试");
                    }

                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //缓存所有页面
                        MemoryCache cache = MemoryCache.Default;
                        cache.Add("FirstPageView", new FirstPageView(), new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
                        cache.Add("PointsSearchView", new PointsSearchView(), new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
                        cache.Add("SongsView", new SongsView(), new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
                        dataLoadedSource.SetResult(true); //发送缓存结束
                    }));
                    await dataLoadedSource.Task; //等待缓存结束
                    //登录成功
                    Messenger.Default.Send(new LoginSuccessMessage());
                    await Task.Delay(800);
                    Application.Current.Dispatcher.Invoke(new Action(() => { (o as Window).DialogResult = true; })); //这行代码会关闭登录窗口
                }
                catch (Exception ex)
                {
                    this.ShowProgress = Visibility.Collapsed;
                    this.ErrorMessage = ex.Message;
                    loginDataSource.SetResult(true);
                    dataLoadedSource.SetResult(true);
                }
            })).ContinueWith(async task =>
            {
                await loginDataSource.Task;
                await dataLoadedSource.Task;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DoLoginButtonRecover(bt);
                });
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }


}
