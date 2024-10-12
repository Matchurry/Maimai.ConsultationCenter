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
using Google.Protobuf.WellKnownTypes;
using LibVLCSharp.Shared;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media.Animation;
using System.Net.NetworkInformation;
using MaimaiConsulationCenter.Model;
using GalaSoft.MvvmLight.Messaging;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// SongsView.xaml 的交互逻辑
    /// </summary>
    public partial class SongsView : UserControl, IDataLoadable
    {
        public static readonly RoutedEvent AfterClickNewSong = EventManager.RegisterRoutedEvent(
            "AfterClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBlock));
        public static readonly RoutedEvent AfterClickNewSongImg = EventManager.RegisterRoutedEvent(
            "AfterClickImg", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Image));
        public static readonly RoutedEvent AfterClickNewSongBd = EventManager.RegisterRoutedEvent(
            "AfterClickBd", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Border));

        public SongsView()
        {
            InitializeComponent();
            InitializeDataAsync();
            Messenger.Default.Register<SongClick>(this, OnSongClick);
        }
        public async Task InitializeDataAsync()
        {
            await new SongsViewModel().GetSongsDataAsync();
            this.DataContext = GlobalValues.SongsModel;
        }

        private void OnSongClick(SongClick e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                TitleTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                ARTIST.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                AristTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                BPM.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                BpmTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                FROM.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                FromTB.RaiseEvent(new RoutedEventArgs(AfterClickNewSong));
                ImgBD.RaiseEvent(new RoutedEventArgs(AfterClickNewSongBd));
                Img.RaiseEvent(new RoutedEventArgs(AfterClickNewSongImg));
            }));
        }

        private double Maximum = 101;
        private double Minimum = 97;
        private double Interval = 5;
        private string[] texts = { "97%", "98%" , "99%", "100%", "100.5%", "101%"};
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
    }
}
