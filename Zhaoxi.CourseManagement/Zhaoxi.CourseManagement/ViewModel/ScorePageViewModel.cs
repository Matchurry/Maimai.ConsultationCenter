using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Zhaoxi.CourseManagement.Model;
using static System.Net.WebRequestMethods;
using static Zhaoxi.CourseManagement.Model.MaiUserScoresModel;

namespace Zhaoxi.CourseManagement.ViewModel
{
    public class ScorePageViewModel
    {
        public Root GetScorePageData()
        {
            var client = new RestClient("https://www.diving-fish.com/api/maimaidxprober/query/player");
            //client.Timeout = -1;
            var request = new RestRequest("", RestSharp.Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
                " + "\n" +
                @"    ""b50"": true,
                " + "\n" +
                @"    ""username"": ""AkiraX""
                " + "\n" +
                            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine("玩家数据获取成功");
            Root userMaiData = JsonConvert.DeserializeObject<Root>(response.Content); //从这开始已经转换为内部Model


            string jsonFilePath = Path.Combine(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."), // 向上返回两级目录
                @"Assets\MaiMusicData\MusicData.json"
            );
            Console.WriteLine(jsonFilePath);
            //读入歌曲json并反序列化
            string jsonFile = System.IO.File.ReadAllText(jsonFilePath);
            ObservableCollection <SongModel.Root> songDatas = JsonConvert.DeserializeObject<ObservableCollection<SongModel.Root>>(jsonFile);

            var cnt = 1;
            foreach(var item in userMaiData.charts.dx)
            {
                item.Zindex = 15 - cnt;
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
                item.id = cnt++;
                item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                item.type_src = String.Format("../Assets/Images/MaiType/{0}.png",item.type);
                item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                if (item.fc == "app") item.fc_src = "../Assets/Images/MaiFcAp/ap.png";
                item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                if (item.fs == "fdsp") item.fs_src = "../Assets/Images/MaiFcAp/fsd.png";

                //获取歌曲的dxScore上限
                var foundSong = songDatas.FirstOrDefault(song => song.id == item.song_id.ToString());
                var cal = 0;
                //随后获取总note数 *3 即为dxScore

                foreach(var notescnt in foundSong.charts[item.level_index].notes){
                    cal += notescnt;
                }

                item.maxDxScore = cal*3;
                item.dx_max_str = item.dxScore.ToString() + " / " + item.maxDxScore.ToString();
                double rate = (double)item.dxScore / item.maxDxScore;
                var rate_level = 0;
                if(rate>=0.85)
                    rate_level = 1;
                if(rate>=0.90)
                    rate_level = 2;
                if(rate>=0.93)
                    rate_level = 3;
                if(rate>=0.95)
                    rate_level = 4;
                if(rate>=0.97)
                    rate_level = 5;
                item.dx_src = String.Format("../Assets/Images/MaiDxScoreRank/{0}.png", rate_level);

            }

            cnt = 1;
            foreach (var item in userMaiData.charts.sd)
            {
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
                item.id = cnt++;
                item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                item.type_src = String.Format("../Assets/Images/MaiType/{0}.png", item.type);
                item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                if (item.fc == "app") item.fc_src = "../Assets/Images/MaiFcAp/ap.png";
                item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                if (item.fs == "fdsp") item.fs_src = "../Assets/Images/MaiFcAp/fsd.png";

                //获取歌曲的dxScore上限
                var foundSong = songDatas.FirstOrDefault(song => song.id == item.song_id.ToString());
                var cal = 0;
                //随后获取总note数 *3 即为dxScore
                foreach (var notescnt in foundSong.charts[item.level_index].notes)
                {
                    cal += notescnt;
                }
                item.maxDxScore = cal * 3;
                item.dx_max_str = item.dxScore.ToString() + " / " + item.maxDxScore.ToString();
                double rate = (double)item.dxScore / item.maxDxScore;
                var rate_level = 0;
                if (rate >= 0.85)
                    rate_level = 1;
                if (rate >= 0.90)
                    rate_level = 2;
                if (rate >= 0.93)
                    rate_level = 3;
                if (rate >= 0.95)
                    rate_level = 4;
                if (rate >= 0.97)
                    rate_level = 5;
                item.dx_src = String.Format("../Assets/Images/MaiDxScoreRank/{0}.png", rate_level);
            }

            return userMaiData;
        }
    }
}
