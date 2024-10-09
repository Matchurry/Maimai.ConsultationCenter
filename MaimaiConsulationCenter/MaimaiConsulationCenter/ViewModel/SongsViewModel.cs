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

namespace MaimaiConsulationCenter.ViewModel
{
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
