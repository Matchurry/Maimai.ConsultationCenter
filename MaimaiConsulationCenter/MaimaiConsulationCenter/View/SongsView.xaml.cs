using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaimaiConsulationCenter.View
{
    public class RegisterRoutedEventExtension : MarkupExtension
    {
        public string EventName { get; set; }
        public Type EventType { get; set; }
        public Type OwnerType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var routedEvents = EventManager.GetRoutedEventsForOwner(OwnerType);
            foreach (RoutedEvent item in routedEvents)
            {
                if (item.Name == EventName) return item;
            }
            throw new ArgumentException($"未找到名为 '{EventName}' 的路由事件，类型为 '{OwnerType.FullName}'。");
        }
    }

    public partial class SongsView : UserControl
    {

        public static readonly RoutedEvent AfterClickNewSong = EventManager.RegisterRoutedEvent(
            "AfterClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBlock));
        public static readonly RoutedEvent AfterClickNewSongImg = EventManager.RegisterRoutedEvent(
            "AfterClickImg", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Image));
        public static readonly RoutedEvent AfterClickNewSongBd = EventManager.RegisterRoutedEvent(
            "AfterClickBd", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Border));
        public static readonly RoutedEvent AfterClickNewDifRight = EventManager.RegisterRoutedEvent(
            "AfterDifGdRight", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Grid));
        public static readonly RoutedEvent AfterClickNewDifLeft = EventManager.RegisterRoutedEvent(
            "AfterDifGdLeft", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Grid));
        public static readonly RoutedEvent AfterClickNewSongForDifShow = EventManager.RegisterRoutedEvent(
            "AfterClickForDifShow", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Grid));
        public static readonly RoutedEvent AfterClickNewSongForDifShowOld = EventManager.RegisterRoutedEvent(
            "AfterClickForDifShowOld", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Grid));

        public static readonly RoutedEvent AfterClickNewEli = EventManager.RegisterRoutedEvent(
            "AfterClickEli", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Ellipse));
        public static readonly RoutedEvent AfterClickNewCan = EventManager.RegisterRoutedEvent(
            "AfterClickCan", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Canvas));
        public SongsView()
        {
            InitializeComponent();
            InitializeDataAsync();
            Messenger.Default.Register<SongClick>(this, OnSongClick);
            Messenger.Default.Register<DifClick>(this, OnDifClick);
        }
        public async Task InitializeDataAsync()
        {
            await new SongsViewModel().GetSongsDataAsync();
            this.DataContext = GlobalValues.SongsModel;
        }

        private void OnSongClick(SongClick e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                TitleTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                ARTIST.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                AristTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                BPM.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                BpmTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                FROM.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                FromTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                ImgBD.RaiseEvent(new RoutedEventArgs(AfterClickNewSongBd));
                Img.RaiseEvent(new RoutedEventArgs(AfterClickNewSongImg));

                breakpointer.RaiseEvent(new RoutedEventArgs(AfterClickNewCan));
                pointer.RaiseEvent(new RoutedEventArgs(AfterClickNewCan));
            }));
            if (GlobalValues.is_first_lauch)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    DifVeryGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShow));
                    DifSecGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShow));
                    NotesGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShow));
                    backEllipse.RaiseEvent(new RoutedEventArgs(AfterClickNewEli));
                    mainCanvas.RaiseEvent(new RoutedEventArgs(AfterClickNewCan));
                }));
                GlobalValues.is_first_lauch = false;
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    DifVeryGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShowOld));
                    DifSecGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShowOld));
                    NotesGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewSongForDifShowOld));
                }));
            }
        }

        private void OnDifClick(DifClick e)
        {
            if (GlobalValues.next_dif_dic == 0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    NotesGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewDifLeft));
                }));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    NotesGrid.RaiseEvent(new RoutedEventArgs(AfterClickNewDifRight));
                }));
            }
            breakpointer.RaiseEvent(new RoutedEventArgs(AfterClickNewCan));
            pointer.RaiseEvent(new RoutedEventArgs(AfterClickNewCan));
        }

        private double Maximum = 101;
        private double Minimum = 97;
        private double Interval = 5;
        private string[] texts = { "97%", "98%", "99%", "100%", "100.5%", "101%" };
        private void CircleDraw(object sender, RoutedEventArgs e)
        {
            if (double.IsNaN(mainCanvas.Width)) return;

            //确定中心位置
            double radius = mainCanvas.Width / 2;
            mainCanvas.Children.Clear(); //避免初始化的时候的绘图，也就是只认最后一次画的
            double step = 90.0 / 50;
            for (int i = 0; i <= 70; i++) //短线
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 18) * Math.Cos((i * step - 63) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 18) * Math.Sin((i * step - 63) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 63) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 63) * Math.PI / 180);

                lineScale.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFB16F"));
                lineScale.StrokeThickness = 1;
                mainCanvas.Children.Add(lineScale);

            }

            step = 90.0 / Interval;
            int scaleText = (int)Minimum;
            for (int i = 0; i <= Interval; i++) //长线
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 25) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 25) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);

                lineScale.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFB16F"));
                lineScale.StrokeThickness = 1.5;
                mainCanvas.Children.Add(lineScale);

                TextBlock textScale = new TextBlock();
                textScale.Width = 50;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = 15;

                textScale.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFB16F"));
                textScale.Text = texts[i];
                Canvas.SetLeft(textScale, radius - (radius - 45) * Math.Cos((i * step - 45) * Math.PI / 180) - 17);
                Canvas.SetTop(textScale, radius - (radius - 45) * Math.Sin((i * step - 45) * Math.PI / 180) - 10);
                this.mainCanvas.Children.Add(textScale);
            }

        }

        private void BasicClick(object sender, MouseEventArgs e)
        {
            if (GlobalValues.now_dif_index != 0)
            {
                GlobalValues.next_dif_dic = 0;
                GlobalValues.now_dif_index = 0;
                Messenger.Default.Send(new DifClick());
            }
        }
        private void AdvClick(object sender, MouseEventArgs e)
        {
            if (GlobalValues.now_dif_index != 1)
            {
                if (1 < GlobalValues.now_dif_index)
                    GlobalValues.next_dif_dic = 0;
                else GlobalValues.next_dif_dic = 1;
                GlobalValues.now_dif_index = 1;
                Messenger.Default.Send(new DifClick());
            }
        }
        private void HardClick(object sender, MouseEventArgs e)
        {
            if (GlobalValues.now_dif_index != 2)
            {
                if (2 < GlobalValues.now_dif_index)
                    GlobalValues.next_dif_dic = 0;
                else GlobalValues.next_dif_dic = 1;
                GlobalValues.now_dif_index = 2;
                Messenger.Default.Send(new DifClick());
            }
        }
        private void MasterClick(object sender, MouseEventArgs e)
        {
            if (GlobalValues.now_dif_index != 3)
            {
                if (3 < GlobalValues.now_dif_index)
                    GlobalValues.next_dif_dic = 0;
                else GlobalValues.next_dif_dic = 1;
                GlobalValues.now_dif_index = 3;
                Messenger.Default.Send(new DifClick());
            }
        }
        private void RemasClick(object sender, MouseEventArgs e)
        {
            if (GlobalValues.now_dif_index != 4)
            {
                if (4 < GlobalValues.now_dif_index)
                    GlobalValues.next_dif_dic = 0;
                else GlobalValues.next_dif_dic = 1;
                GlobalValues.now_dif_index = 4;
                Messenger.Default.Send(new DifClick());
            }
        }
    }
}
