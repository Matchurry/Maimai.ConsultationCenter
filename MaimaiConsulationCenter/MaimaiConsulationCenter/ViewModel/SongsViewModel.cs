﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using static MaimaiConsulationCenter.Model.SongModel;
using System.Net;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Web.WebSockets;

namespace MaimaiConsulationCenter.ViewModel
{
    public class SongClick { }; //切换到新的曲子时
    public class DifClick { }; //谱面难度切换时
    /// <summary>
    /// 用于提示玩家能否吃分的文字行为
    /// </summary>
    public class RateHintText : Behavior<TextBlock>
    {
        private TranslateTransform trans = new TranslateTransform();
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, Rota);
            Messenger.Default.Register<DifClick>(this, RotaDif);
            trans.Y = 50;
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        trans
                    }
            };
        }
        private async void Rota(SongClick e)
        {
            double tarRate = 0;
            /// RANK 系数表
            /// 单曲Rating = 定数 * 系数 * 达成率 | 取整
            ///     达成率     系数      倍率
            ///     100.5     22.4      22.512
            ///     100.4999  22.2      22.310
            ///     100.0000  21.6      21.707-21.600
            ///     99.9999   21.4      21.399
            ///     99.5000   21.1      21.099-20.994
            ///     99.0000   20.8      20.695-20.592
            ///     98.0000   20.3      20.096-19.894
            ///     97.0000   20.0      19.599-19.400
            double floor = 0;
            if (GlobalValues.SingleSongShow.type == "SD") floor = GlobalValues.B35Floor + 1;
            else floor = GlobalValues.B15Floor + 1;
            floor /= GlobalValues.SingleSongShow.ds[GlobalValues.now_dif_index];

            var rani = new DoubleAnimation(50, TimeSpan.FromSeconds(0.2));
            rani.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut };
            trans.BeginAnimation(TranslateTransform.YProperty, rani);
            await Task.Delay(200);
            if (floor > 22.512 )
                AssociatedObject.Text = $"该谱面对应的难度您已经无法吃分了哦！";
            else if(floor < 19.400)
                AssociatedObject.Text = $"该谱面对应的难度对您来说有点大了...";
            else
            {
                if (floor > 21.707) tarRate = 1.005;
                else if (floor > 21.600) tarRate = floor / 21.6;
                else if (floor > 20.994) tarRate = floor / 21.4;
                else if (floor > 20.592) tarRate = floor / 21.1;
                else if (floor > 19.894) tarRate = floor / 20.8;
                else tarRate = floor / 20.0;
                tarRate *= 100;
                if(floor > 21.707)
                    AssociatedObject.Text = $"向着SSS+迸发吧！";
                else
                    AssociatedObject.Text = $"达到{tarRate:F4}%达成率以吃分！";
            }
            rani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            rani.BeginTime = TimeSpan.FromSeconds(0.2);
            rani.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut };
            trans.BeginAnimation(TranslateTransform.YProperty, rani);

        }
        private void RotaDif(DifClick e)
        {
            Rota(new SongClick());
        }
    }
    /// <summary>
    /// 用于显示玩家目标成绩刻度的动画行为
    /// </summary>
    public class BreakPointerRota : Behavior<Canvas>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, Rota);
            Messenger.Default.Register<DifClick>(this, RotaDif);
        }
        private void Rota(SongClick e)
        {
            var rota = new RotateTransform();
            rota.Angle = -100.0;
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        rota
                    }
            };

            double tarRate = 0;
            /// RANK 系数表
            /// 单曲Rating = 定数 * 系数 * 达成率 | 取整
            ///     达成率     系数      倍率
            ///     100.5     22.4      22.512
            ///     100.4999  22.2      22.310
            ///     100.0000  21.6      21.707-21.600
            ///     99.9999   21.4      21.399
            ///     99.5000   21.1      21.099-20.994
            ///     99.0000   20.8      20.695-20.592
            ///     98.0000   20.3      20.096-19.894
            ///     97.0000   20.0      19.599-19.400
            double floor = 0;
            if (GlobalValues.SingleSongShow.type == "SD") floor = GlobalValues.B35Floor + 1;
            else floor = GlobalValues.B15Floor + 1;
            floor /= GlobalValues.SingleSongShow.ds[GlobalValues.now_dif_index];

            if (floor > 22.512 || floor < 19.400) return;
            else
            {
                if (floor > 21.707) tarRate = 1.005;
                else if (floor > 21.600) tarRate = floor / 21.6;
                else if (floor > 20.994) tarRate = floor / 21.4;
                else if (floor > 20.592) tarRate = floor / 21.1;
                else if (floor > 19.894) tarRate = floor / 20.8;
                else tarRate = floor / 20.0;
            }

            tarRate *= 100;
            Console.WriteLine(tarRate);

            double ro = -100.0;
            if (tarRate >= 100.0)
                ro = 9 + (tarRate - 100) * 36;
            else
                ro = -45 - 18 + (tarRate - 96) * 18;
            var rani = new DoubleAnimation(ro, TimeSpan.FromSeconds(2));
            rani.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut };
            rota.BeginAnimation(RotateTransform.AngleProperty, rani);
        }
        private void RotaDif(DifClick e)
        {
            Rota(new SongClick());
        }
    }
    /// <summary>
    /// 用于显示玩家成绩刻度的动画行为
    /// </summary>
    public class PointerRota : Behavior<Canvas>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, Rota);
            Messenger.Default.Register<DifClick>(this, RotaDif);
        }
        private void Rota(SongClick e)
        {
            var rota = new RotateTransform();
            rota.Angle = -100.0;
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        rota
                    }
            };
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements < 96.0) return;
            double ro = -100.0;
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements >= 100.0)
                ro = 9 + (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements - 100) * 36;
            else
                ro = -45-18 + (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements - 96) * 18;
            var rani = new DoubleAnimation(ro, TimeSpan.FromSeconds(2));
            rani.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut};
            rota.BeginAnimation(RotateTransform.AngleProperty, rani);
        }
        private void RotaDif(DifClick e)
        {
            Rota(new SongClick());
        }
    }
    public class MultiSongsShow : Behavior<Grid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            (AssociatedObject as Grid).MouseDown += NewSong;
        }
        protected override void OnDetaching()
        {
            (AssociatedObject as Grid).MouseDown -= NewSong;
            base.OnDetaching();
        }
        private void NewSong(object sender, MouseButtonEventArgs e)
        {
            var obj = (AssociatedObject as Grid).DataContext;
            if(obj is SongModel.Root songItem)
            {
                if (GlobalValues.SingleSongShow.id != null && songItem.id == GlobalValues.SingleSongShow.id) return;
                GlobalValues.SingleSongShow = songItem;
                GlobalValues.now_dif_index = songItem.ds.Count-1;
            }
            Messenger.Default.Send(new SongClick());
        }
    }
    public class NewFidForNewColor : Behavior<Border>
    {
        private string[] difcolors = { "#70D43E", "#F9B709", "#FE818D", "#9D51DD", "#DAAADF" };
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeColor);
            Messenger.Default.Register<DifClick>(this, ChangeColorDif);
        }
        private void ChangeColor(SongClick e)
        {
            ChangeColorDif(new DifClick());
        }
        private void ChangeColorDif(DifClick e)
        {
            ColorAnimation animation = new ColorAnimation();
            animation.To = (Color)ColorConverter.ConvertFromString(difcolors[GlobalValues.now_dif_index]);
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            AssociatedObject.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
    public class RateImgChange : Behavior<Image>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements >= 98.0)
                AssociatedObject.Source = new BitmapImage(new Uri($"../Assets/Images/MaiRanks/{GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].rate}.png", UriKind.Relative));
            else
                AssociatedObject.Source = new BitmapImage(new Uri("../Assets/Images/null.png", UriKind.Relative));
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class RateFsImgChange : Behavior<Image>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].fs != null)
                AssociatedObject.Source = new BitmapImage(new Uri($"../Assets/Images/MaiFsFDX/{GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].fs}.png", UriKind.Relative));
            else
                AssociatedObject.Source = new BitmapImage(new Uri("../Assets/Images/null.png", UriKind.Relative));
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class RateFcImgChange : Behavior<Image>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].fc != null)
                AssociatedObject.Source = new BitmapImage(new Uri($"../Assets/Images/MaiFcAp/{GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].fc}.png", UriKind.Relative));
            else
                AssociatedObject.Source = new BitmapImage(new Uri("../Assets/Images/null.png", UriKind.Relative));
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class RateChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (!GlobalValues.is_pwd)
            {
                AssociatedObject.Text = "尚未登录";
                return;
            }
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements != 0.0)
                AssociatedObject.Text = $"{GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].achivements}%";
            else
                AssociatedObject.Text = "尚未游玩";
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class BreakNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes.Count < 5)
                AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[3].ToString();
            else AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[4].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class TouchNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes.Count < 5)
                AssociatedObject.Text = "0";
            else AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[3].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class SlideNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[2].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class HoldNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[1].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class TapNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[0].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class TotalNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            var total = 0;
            foreach(var item in GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes)
            {
                total += item;
            }
            await Task.Delay(100);
            AssociatedObject.Text = total.ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    /// <summary>
    /// 左侧虚拟化全曲展示列的行为
    /// </summary>
    public class NoterDifChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].charter;
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    public class LevelDifChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
            Messenger.Default.Register<DifClick>(this, ChangeTextDif);
        }
        private async void ChangeText(SongClick e)
        {
            await Task.Delay(100);
            AssociatedObject.Text = GlobalValues.SingleSongShow.ds[GlobalValues.now_dif_index].ToString();
        }
        private void ChangeTextDif(DifClick e)
        {
            ChangeText(new SongClick());
        }
    }
    /// <summary>
    /// 这是谱面难度栏下方指示条的行为定义
    /// </summary>
    public class DifBoderChange : Behavior<Border>
    {
        private static TranslateTransform translateTransform;
        private double XReMas = 325;
        private double XMas = 300;
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangePos);
            Messenger.Default.Register<DifClick>(this, ChangePosAni);
        }
        private async void ChangePos(SongClick e)
        {
            translateTransform = new TranslateTransform();
            if (GlobalValues.SingleSongShow.charts.Count==5)
            {
                translateTransform.X = XReMas;
            }
            else
            {
                translateTransform.X = XMas;
            }
            await Task.Delay(100);
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        translateTransform
                    }
            };
        }
        private void ChangePosAni(DifClick e)
        {
            var x = 0.0;
            if (GlobalValues.SingleSongShow.charts.Count == 5)
            {
                x = XReMas / 4 * GlobalValues.now_dif_index;
            }
            else
            {
                x = XMas / 3 * GlobalValues.now_dif_index;
            }
            var xani = new DoubleAnimation(x,TimeSpan.FromSeconds(0.1));
            translateTransform.BeginAnimation(TranslateTransform.XProperty,xani);
        }
    }
    /// <summary>
    /// 这是谱面难度选择时候 根据曲子是有否REMAS难度自动分配空间的行为定义
    /// </summary>
    public class DifChange : Behavior<ColumnDefinition>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeWidth);
        }
        private async void ChangeWidth(SongClick e)
        {
            await Task.Delay(100);
            if(GlobalValues.now_dif_index == 4)
            {
                AssociatedObject.Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                AssociatedObject.Width = new GridLength(0, GridUnitType.Star);
            }
        }
    }
    public class TitleChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.title;
        }
    }
    public class ImageChange : Behavior<Image>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Source = new BitmapImage(new Uri(GlobalValues.SingleSongShow.song_img_src, UriKind.Relative));
        }
    }
    public class ArtistChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.basic_info.artist;
        }
    }
    public class BpmChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.basic_info.bpm.ToString();
        }
    }
    public class FromChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.basic_info.from;
        }
    }
    public class SongsViewModel
    {
        public async Task GetSongsDataAsync()
    {
        if (GlobalValues.SongsModel == null)
        {
            await Task.Run(() =>
            {
                string jsonFilePath = Path.Combine(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."),
                    @"Assets\MaiMusicData\MusicData.json");
                string jsonFile = System.IO.File.ReadAllText(jsonFilePath);
                ObservableCollection<SongModel.Root> songDatas = JsonConvert.DeserializeObject<ObservableCollection<SongModel.Root>>(jsonFile);
                GlobalValues.SongsModel = songDatas;
            });
        }

        string imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..","Assets", "Images", "MaiSongImages");
        foreach (var item in GlobalValues.SongsModel)
        {
            item.song_img_src = String.Format("../Assets/Images/MaiSongImages/{0}.png", item.id.PadLeft(5, '0'));
            string imagePath = Path.Combine(imageDirectory, $"{item.id.PadLeft(5, '0')}.png");
            if (!System.IO.File.Exists(imagePath))
                item.song_img_src = "../Assets/Images/null.png";

/*                string url = String.Format("https://www.diving-fish.com/covers/{0}.png", item.id.PadLeft(5, '0'));
                // 下载图片到指定的目录
                try
                {
                    if (!File.Exists(imagePath))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(url, imagePath);
                            Console.WriteLine(item.id.PadLeft(5, '0'));
                        }
                    }
                }
                catch { }*/


                int cnt = 0;
            foreach(var dif in item.ds)
            {
                switch (cnt)
                {
                    case 0:
                        item.easy = dif.ToString();
                        break;
                    case 1:
                        item.advanced = dif.ToString();
                        break;
                    case 2:
                        item.hard = dif.ToString();
                        break;
                    case 3:
                        item.master = dif.ToString();
                        break;
                    case 4:
                        item.remaster = dif.ToString();
                        break;
                }
                cnt++;
            }
        }

        return;
    }
    }
}
