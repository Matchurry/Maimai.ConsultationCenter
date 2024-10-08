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
                });
            }
            return;
        }
    }
}
