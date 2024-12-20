﻿using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static MaimaiConsulationCenter.Model.MaiUserScoresModel;

namespace MaimaiConsulationCenter.ViewModel
{
    public class B35MouseEnterMessage { };
    public class B15MouseEnterMessage { };
    public class B50MouseLeaveMessage { };

    public class B35MouseEnterBehavior : Behavior<FrameworkElement>
    {
        private int thisid;
        private object obj;
        private TranslateTransform translateTransform = new TranslateTransform();
        private ScaleTransform scaleTransform = new ScaleTransform();
        protected override void OnAttached()
        {
            base.OnAttached();
            obj = (AssociatedObject as Grid).DataContext;
            if (obj is MaiUserScoresModel.Sd dxItem)
            {
                thisid = dxItem.id;
            }
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                {
                    translateTransform,
                    scaleTransform
                }
            };
            Messenger.Default.Register<B35MouseEnterMessage>(this, Resize);
            Messenger.Default.Register<B50MouseLeaveMessage>(this, Recover);
        }

        private void Resize(B35MouseEnterMessage msg)
        {
            double tarsX = 1, tarsY = 1, tarX = 0, tarY = 0;
            var subid = GlobalValues.B35_UI_Id - thisid;
            switch (subid)
            {
                case 0:
                    tarsX = 1.3;
                    tarsY = 1.3;
                    break;
                case -5:
                    tarY = 10;
                    break;
                case 5:
                    tarY = -10;
                    break;
                case -1:
                    if (GlobalValues.B35_UI_Id % 5 != 0)
                    {
                        tarX = 25;
                    }
                    break;
                case 1:
                    if ((GlobalValues.B35_UI_Id - 1) % 5 != 0)
                    {
                        tarX = -25;
                    }
                    break;
                default:
                    return;
            }

            var xAnimation = new DoubleAnimation(tarX, TimeSpan.FromSeconds(0.1));
            var yAnimation = new DoubleAnimation(tarY, TimeSpan.FromSeconds(0.1));
            var xScaleAnimation = new DoubleAnimation(tarsX, TimeSpan.FromSeconds(0.1));
            var yScaleAnimation = new DoubleAnimation(tarsY, TimeSpan.FromSeconds(0.1));
            translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, xScaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, yScaleAnimation);
        }

        private void Recover(B50MouseLeaveMessage msg)
        {
            double tarsX = 1, tarsY = 1, tarX = 0, tarY = 0;
            var subid = GlobalValues.B15_UI_Id - thisid;

            var xAnimation = new DoubleAnimation(tarX, TimeSpan.FromSeconds(0.1));
            var yAnimation = new DoubleAnimation(tarY, TimeSpan.FromSeconds(0.1));
            var xScaleAnimation = new DoubleAnimation(tarsX, TimeSpan.FromSeconds(0.1));
            var yScaleAnimation = new DoubleAnimation(tarsY, TimeSpan.FromSeconds(0.1));
            translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, xScaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, yScaleAnimation);
        }
    }

    public class B15MouseEnterBehavior : Behavior<FrameworkElement>
    {
        private int thisid;
        private object obj;
        private TranslateTransform translateTransform = new TranslateTransform();
        private ScaleTransform scaleTransform = new ScaleTransform();
        protected override void OnAttached()
        {
            base.OnAttached();
            obj = (AssociatedObject as Grid).DataContext;
            if (obj is MaiUserScoresModel.Dx dxItem)
            {
                thisid = dxItem.id;
            }
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                {
                    translateTransform,
                    scaleTransform
                }
            };
            Messenger.Default.Register<B15MouseEnterMessage>(this, Resize);
            Messenger.Default.Register<B50MouseLeaveMessage>(this, Recover);
        }

        private void Resize(B15MouseEnterMessage msg)
        {
            double tarsX = 1, tarsY = 1, tarX = 0, tarY = 0;
            var subid = GlobalValues.B15_UI_Id - thisid;
            switch (subid)
            {
                case 0:
                    tarsX = 1.3;
                    tarsY = 1.3;
                    break;
                case -5:
                    tarY = 10;
                    break;
                case 5:
                    tarY = -10;
                    break;
                case -1:
                    if (GlobalValues.B15_UI_Id % 5 != 0)
                    {
                        tarX = 25;
                    }
                    break;
                case 1:
                    if ((GlobalValues.B15_UI_Id - 1) % 5 != 0)
                    {
                        tarX = -25;
                    }
                    break;
                default:
                    return;
            }

            var xAnimation = new DoubleAnimation(tarX, TimeSpan.FromSeconds(0.1));
            var yAnimation = new DoubleAnimation(tarY, TimeSpan.FromSeconds(0.1));
            var xScaleAnimation = new DoubleAnimation(tarsX, TimeSpan.FromSeconds(0.1));
            var yScaleAnimation = new DoubleAnimation(tarsY, TimeSpan.FromSeconds(0.1));
            translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, xScaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, yScaleAnimation);
        }
        private void Recover(B50MouseLeaveMessage msg)
        {
            double tarsX = 1, tarsY = 1, tarX = 0, tarY = 0;
            var subid = GlobalValues.B15_UI_Id - thisid;

            var xAnimation = new DoubleAnimation(tarX, TimeSpan.FromSeconds(0.1));
            var yAnimation = new DoubleAnimation(tarY, TimeSpan.FromSeconds(0.1));
            var xScaleAnimation = new DoubleAnimation(tarsX, TimeSpan.FromSeconds(0.1));
            var yScaleAnimation = new DoubleAnimation(tarsY, TimeSpan.FromSeconds(0.1));
            translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, xScaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, yScaleAnimation);
        }
    }

    /*    public class ResizeByMouseBehavior : Behavior<FrameworkElement>
        {
            private DispatcherTimer timer;
            private static Point mousePosition;
            private Point _lastMousePosition;
            private static double _maxDis = Math.Sqrt(216 * 216 + 100 * 100) / 2;

            protected override void OnAttached()
            {
                base.OnAttached();
                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            protected override void OnDetaching()
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                base.OnDetaching();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                try
                {
                    // 获取鼠标位置
                    Point currentMousePosition = Mouse.GetPosition(Application.Current.MainWindow);
                    // 将鼠标位置转换为屏幕坐标系下的坐标
                    mousePosition = Application.Current.MainWindow.PointToScreen(currentMousePosition);
                    //mousePosition = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject)); //屏幕坐标系下鼠标的位置
                   if (!(Math.Abs(mousePosition.X - _lastMousePosition.X) > Threshold ||
                        Math.Abs(mousePosition.Y - _lastMousePosition.Y) > Threshold))
                    {
                        _lastMousePosition = mousePosition;
                        return;
                    }
                    _lastMousePosition = mousePosition;
                    Point objectScreenPosition = AssociatedObject.PointToScreen(new Point(0, 0));
                    double y = mousePosition.Y - (objectScreenPosition.Y + 100 / 2); // y距离 有正负
                    double x = mousePosition.X - (objectScreenPosition.X + 216 / 2); // x距离 有正负
                    double distance = Math.Sqrt(x * x + y * y);
                    double tarsX = 1, tarsY = 1, tarX = 0, tarY = 0;
                    //动画1 鼠标在控件内部时 控件放大
                    if (Math.Abs(x) <= 216 / 2 && Math.Abs(y) <= 100 / 2) //鼠标在控件内 应用缩放
                    {
                        //变为原来的1.3倍
                        tarsX = 1.3;
                        tarsY = 1.3;
                    }

                    //动画2 对于同一行布局X大小的变换
                    if (Math.Abs(y) <= 100 / 2) //如果鼠标在我这一行
                    {
                        if(Math.Abs(x) >= 226/2  && Math.Abs(x) <= 206 + 226/2) //鼠标在我之外一个范围内
                        {
                            var cal = Math.Abs(x);
                            tarX = 25;
                            if (x>0) tarX = -tarX;
                        }
                    }

                    //动画3 对于同一列布局Y大小的变换
                    if (Math.Abs(x) <= 216 / 2) //如果鼠标在我这一行
                    {
                        if (Math.Abs(y) >= 110 / 2 && Math.Abs(y) <= 90 + 110 / 2) //鼠标在我之外一个范围内
                        {
                            var cal = Math.Abs(y);
                            tarY = 10;
                            if (y > 0) tarY = -tarY;
                        }
                    }

                    AssociatedObject.RenderTransform = new TransformGroup
                    {
                        Children =
                            {
                                new TranslateTransform {X = tarX, Y = tarY},
                                new ScaleTransform { ScaleX = tarsX, ScaleY=tarsY },
                            }
                    };

                }
                catch(System.InvalidOperationException){
                    timer.Stop();
                    timer.Tick -= Timer_Tick;
                }

            }

        }*/

    public class ScorePageViewModel : NotifyBase
    {
        public Root GetScorePageDataAsync()
        {
            Root userMaiData = GlobalValues.B50;
            var cnt = 1; string imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Assets", "Images", "MaiSongImages");
            if (userMaiData.charts != null && userMaiData.charts.dx.Count != 0)
                foreach (var item in userMaiData.charts.dx)
                {
                    item.Zindex = 15 - cnt;
                    item.song_img_src = String.Format("../Assets/Images/MaiSongImages/{0:D5}.png", item.song_id);
                    string imagePath = Path.Combine(imageDirectory, String.Format("{0:D5}.png", item.song_id));
                    if (!System.IO.File.Exists(imagePath))
                        item.song_img_src = "../Assets/Images/null.png";
                    item.id = cnt++;
                    item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                    item.type_src = String.Format("../Assets/Images/MaiType/{0}.png", item.type);
                    item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                    item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                    if (item.rate == "")
                        item.rate_src = "../Assets/Images/MaiRanks/null.png";
                    if (item.fc == "")
                        item.fc_src = "../Assets/Images/MaiFcAp/null.png";
                    if (item.fs == "")
                        item.fs_src = "../Assets/Images/MaiFsFDX/null.png";
                    item.animationlengh = string.Format("0:0:{0}.{1}", item.id / 10 + 1, item.id % 10);

                    //获取歌曲的dxScore上限
                    var foundSong = GlobalValues.SongsModel.FirstOrDefault(song => song.id == item.song_id.ToString());
                    var cal = 0;
                    //随后获取总note数 *3 即为dxScore

                    foreach (var notescnt in foundSong.charts[item.level_index].notes)
                    {
                        cal += notescnt;
                    }

                    item.maxDxScore = cal * 3;
                    item.dx_max_str = item.dxScore.ToString() + " / " + item.maxDxScore.ToString();
                    double rate = (double)item.dxScore / item.maxDxScore;
                    var rate_level = "null";
                    if (rate >= 0.85)
                        rate_level = "1";
                    if (rate >= 0.90)
                        rate_level = "2";
                    if (rate >= 0.93)
                        rate_level = "3";
                    if (rate >= 0.95)
                        rate_level = "4";
                    if (rate >= 0.97)
                        rate_level = "5";
                    item.dx_src = String.Format("../Assets/Images/MaiDxScoreRank/{0}.png", rate_level);

                }

            cnt = 1;
            if (userMaiData.charts != null && userMaiData.charts.sd.Count != 0)
                foreach (var item in userMaiData.charts.sd)
                {
                    item.song_img_src = String.Format("../Assets/Images/MaiSongImages/{0:D5}.png", item.song_id);
                    string imagePath = Path.Combine(imageDirectory, String.Format("{0:D5}.png", item.song_id));
                    if (!System.IO.File.Exists(imagePath))
                        item.song_img_src = "../Assets/Images/null.png";
                    item.id = cnt++;
                    item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                    item.type_src = String.Format("../Assets/Images/MaiType/{0}.png", item.type);
                    item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                    item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                    item.animationlengh = string.Format("0:0:{0}.{1}", (item.id + 15) / 10 + 1, (item.id + 15) % 10);
                    if (item.rate == "")
                        item.rate_src = "../Assets/Images/MaiRanks/null.png";
                    if (item.fc == "")
                        item.fc_src = "../Assets/Images/MaiFcAp/null.png";
                    if (item.fs == "")
                        item.fs_src = "../Assets/Images/MaiFsFDX/null.png";
                    //获取歌曲的dxScore上限
                    var foundSong = GlobalValues.SongsModel.FirstOrDefault(song => song.id == item.song_id.ToString());
                    var cal = 0;
                    //随后获取总note数 *3 即为dxScore
                    foreach (var notescnt in foundSong.charts[item.level_index].notes)
                    {
                        cal += notescnt;
                    }
                    item.maxDxScore = cal * 3;
                    item.dx_max_str = item.dxScore.ToString() + " / " + item.maxDxScore.ToString();
                    double rate = (double)item.dxScore / item.maxDxScore;
                    var rate_level = "null";
                    if (rate >= 0.85)
                        rate_level = "1";
                    if (rate >= 0.90)
                        rate_level = "2";
                    if (rate >= 0.93)
                        rate_level = "3";
                    if (rate >= 0.95)
                        rate_level = "4";
                    if (rate >= 0.97)
                        rate_level = "5";
                    item.dx_src = String.Format("../Assets/Images/MaiDxScoreRank/{0}.png", rate_level);
                }

            return userMaiData;
        }
    }
}
