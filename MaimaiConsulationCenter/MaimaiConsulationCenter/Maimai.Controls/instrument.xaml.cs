using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zhaoxi.Controls
{
    public partial class instrument : UserControl
    {
        //依赖属性,依赖对象
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value",typeof(double), typeof(instrument), 
            new PropertyMetadata(0.0,new PropertyChangedCallback(OnPropertyChanged)));


        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(instrument), 
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChanged)));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(instrument), 
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChanged)));

        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(double), typeof(instrument), 
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChanged)));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as instrument).Refresh();
        }

        public instrument()
        {
            InitializeComponent();
            this.SizeChanged += Instrument_SizeChanged;
        }

        private void Instrument_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize = Math.Min(RenderSize.Width, RenderSize.Height);
            backEllipse.Width = minSize;
            backEllipse.Height = minSize;
        }

        private void Refresh()
        {
            if (double.IsNaN(backEllipse.Width)) return;
            //确定中心位置
            double radius = backEllipse.Width / 2;
            mainCanvas.Children.Clear(); //避免初始化的时候的绘图，也就是只认最后一次画的
            //double min = 0, max = 100;
            //double scaleCountArea = 10;
            double step = 270.0 / (Maximum - Minimum);
            for(int i=0; i< Maximum - Minimum; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = radius- (radius-13) * Math.Cos((i * step-45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius-13) * Math.Sin((i * step-45) * Math.PI / 180);
                lineScale.X2 = radius - (radius-8) * Math.Cos((i * step-45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius-8) * Math.Sin((i * step-45) * Math.PI / 180);

                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 1;
                mainCanvas.Children.Add(lineScale);

            }

            step = 270.0 / Interval;
            int scaleText = (int)Minimum;
            for (int i = 0; i <= Interval; i++) {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 20) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 20) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);

                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 1;
                mainCanvas.Children.Add(lineScale);

                TextBlock textScale = new TextBlock();
                textScale.Width = 34;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = 14;
                //textScale.Background = Brushes.Green;
                textScale.Foreground = Brushes.White;
                textScale.Text = (scaleText+(Maximum - Minimum) /Interval *i).ToString();
                textScale.Foreground = Brushes.White;
                Canvas.SetLeft(textScale, radius - (radius - 36) * Math.Cos((i * step - 45) * Math.PI / 180)-17);
                Canvas.SetTop(textScale, radius - (radius - 36) * Math.Sin((i * step - 45) * Math.PI / 180)-10);
                this.mainCanvas.Children.Add(textScale);
            }

            string sData = "M{0} {1} A{0} {0} 0 1 1 {1} {2}";
            sData = string.Format(sData, radius / 2, radius, radius*1.5);
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            circle.Data = (Geometry)converter.ConvertFrom(sData);
            step = 270.0 / (Maximum - Minimum);
            //rtpointer.Angle = Value * step -45;
            //double value = double.IsNaN(Value) ? 0: Value;
            DoubleAnimation da = new DoubleAnimation((Value-Minimum)*step-45, new Duration(TimeSpan.FromMilliseconds(200)));
            rtpointer.BeginAnimation(RotateTransform.AngleProperty, da);

            sData = "M{0} {1},{1} {2},{1} {3}";
            sData = string.Format(sData, radius*0.3, radius, radius-5, radius+5);
            pointer.Data = (Geometry)converter.ConvertFrom(sData);
        }
    }
}
