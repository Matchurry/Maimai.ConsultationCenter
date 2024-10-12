using Newtonsoft.Json;
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

namespace MaimaiConsulationCenter.ViewModel
{
    public class SongClick { };
    /// <summary>
    /// 左侧虚拟化全曲展示列的行为
    /// </summary>
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
    public class BreakNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes.Count < 5)
                AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[3].ToString();
            else AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[4].ToString();
        }
    }
    public class TouchNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            if (GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes.Count < 5)
                AssociatedObject.Text = "0";
            else AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[3].ToString();
        }
    }
    public class SlideNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[2].ToString();
        }
    }
    public class HoldNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[1].ToString();
        }
    }
    public class TapNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes[0].ToString();
        }
    }
    public class TotalNotesChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            var total = 0;
            foreach(var item in GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].notes)
            {
                total += item;
            }
            AssociatedObject.Text = total.ToString();
        }
    }
    public class NoterDifChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.charts[GlobalValues.now_dif_index].charter;
        }
    }
    public class LevelDifChange : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangeText);
        }
        private void ChangeText(SongClick e)
        {
            AssociatedObject.Text = GlobalValues.SingleSongShow.ds[GlobalValues.now_dif_index].ToString();
        }
    }
    /// <summary>
    /// 这是谱面难度栏下方指示条的行为定义
    /// </summary>
    public class DifBoderChange : Behavior<Border>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Messenger.Default.Register<SongClick>(this, ChangePos);
        }
        private void ChangePos(SongClick e)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            if (GlobalValues.now_dif_index == 4)
            {
                translateTransform.X = 330;
            }
            else
            {
                translateTransform.X = 315;
            }
            AssociatedObject.RenderTransform = new TransformGroup
            {
                Children =
                    {
                        translateTransform
                    }
            };
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
        private void ChangeWidth(SongClick e)
        {
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
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."), // 向上返回两级目录
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

            /*                 string url = String.Format("https://www.diving-fish.com/covers/{0}.png", item.id.PadLeft(5,'0'));
                            string imagePath = Path.Combine(imageDirectory, $"{item.id.PadLeft(5, '0')}.png");
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
                            catch{ }*/

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
