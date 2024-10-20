using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MaimaiConsulationCenter.ViewModel
{
    public class NavChange { }
    public class DifForBorAni : Behavior<Border>
    {
        private TranslateTransform tt = new TranslateTransform();
        private string[] difcolors = { "#70D43E", "#F9B709", "#FE818D", "#9D51DD", "#DAAADF" };
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        tt,
                    }
            };
            Messenger.Default.Register<NavChange>(this, BackColor);
            Messenger.Default.Register<SongClick>(this, ClorAni);
            Messenger.Default.Register<DifClick>(this, ClorAniDif);
        }
        private async void ClorAni(SongClick e)
        {
            await Task.Delay(500);
            AssociatedObject.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(difcolors[GlobalValues.now_dif_index]));
        }
        private void ClorAniDif(DifClick e)
        {
            ClorAni(new SongClick());
        }
        private void BackColor(NavChange e)
        {
            var cloAni = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString("#F79645"),
                Duration = TimeSpan.FromSeconds(0.2f)
            };
            AssociatedObject.Background.BeginAnimation(SolidColorBrush.ColorProperty, cloAni);
        }
    }
    public class DifForEliAni : Behavior<Ellipse>
    {
        private TranslateTransform tt = new TranslateTransform();
        private string[] difcolors = { "#70D43E", "#F9B709", "#FE818D", "#9D51DD", "#DAAADF" };
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        tt,
                    }
            };
            Messenger.Default.Register<SongClick>(this, ClorAni);
            Messenger.Default.Register<DifClick>(this, ClorAniDif);
        }
        private async void ClorAni(SongClick e)
        {
            var mouseP = Mouse.GetPosition(null);
            var opaAni = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(1f),
            };
            AssociatedObject.BeginAnimation(Ellipse.OpacityProperty, opaAni);
            AssociatedObject.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(difcolors[GlobalValues.now_dif_index]));
            tt.X = mouseP.X;
            tt.Y = mouseP.Y;
            var WidthHeightAni = new DoubleAnimation
            {
                From = 0,
                To = 2500,
                Duration = TimeSpan.FromSeconds(0.5f),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut }
            };

            var lefttopAni = new DoubleAnimation
            {
                From = 0,
                To = -1250,
                Duration = TimeSpan.FromSeconds(0.5f),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut }
            };
            AssociatedObject.BeginAnimation(Ellipse.WidthProperty, WidthHeightAni);
            AssociatedObject.BeginAnimation(Ellipse.HeightProperty, WidthHeightAni);
            AssociatedObject.BeginAnimation(Canvas.LeftProperty, lefttopAni);
            AssociatedObject.BeginAnimation(Canvas.TopProperty, lefttopAni);

            await Task.Delay(500);
            opaAni = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1f),
            };
            AssociatedObject.BeginAnimation(Ellipse.OpacityProperty, opaAni);
        }
        private void ClorAniDif(DifClick e)
        {
            ClorAni(new SongClick());
        }
    }
    public class MainViewModel : NotifyBase
    {
        public UserModel UserInfo { get; set; } = new UserModel();
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; DoNotify(); }
        }

        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; DoNotify(); }
        }

        public CommandBase NavChangedCommand { get; set; }

        public MainViewModel()
        {
            NavChangedCommand = new CommandBase();
            NavChangedCommand.DoExecute = new Action<object>(DoNavChanged);
            NavChangedCommand.DoCanExecute = new Func<object, bool>((o) => true);
            DoNavChanged("FirstPageView");
        }

        private async void DoNavChanged(object o)
        {
            if (MainContent != null && MainContent.ToString() == "MaimaiConsulationCenter.View." + o.ToString())
                return;

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MainContent = new MaimaiConsulationCenter.View.LoadingView(); // 加载过渡页面
            }));

            MemoryCache cache = MemoryCache.Default;
            FrameworkElement cachedPage = cache.Get(o.ToString()) as FrameworkElement;
            MainContent = cachedPage;

            Messenger.Default.Send(new NavChange());

            /*  时代变了 await Application.Current.Dispatcher.BeginInvoke(new Action(async() =>
                        {
                            Type type = Type.GetType("MaimaiConsulationCenter.View." + o.ToString());
                            ConstructorInfo cti = type.GetConstructor(System.Type.EmptyTypes);
                            var tar = (FrameworkElement)cti.Invoke(null);
                            if(tar is IDataLoadable dataLoadablePage)
                            {
                                await dataLoadablePage.InitializeDataAsync(); //这边等待检测其实也执行了一次 就不需要在初始化的时候再执行了
                            }
                            MainContent = tar;

                        }));*/

        }

    }
}
