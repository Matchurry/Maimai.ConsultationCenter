using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.CourseManagement.Model;
using static System.Net.WebRequestMethods;
using static Zhaoxi.CourseManagement.Model.MaiUserScoresModel;

namespace Zhaoxi.CourseManagement.ViewModel
{
    public class ScorePageViewModel
    {
        public ObservableCollection<MaiUserScoresModel> B15 { get; set; }
        public ObservableCollection<MaiUserScoresModel> B35 { get; set; }

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
                @"    ""username"": ""heiza233""
                " + "\n" +
                            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine("玩家数据获取成功");
            Root userMaiData = JsonConvert.DeserializeObject<Root>(response.Content); //从这开始已经转换为内部Model
            foreach(var item in userMaiData.charts.dx)
            {
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
            }
            foreach (var item in userMaiData.charts.sd)
            {
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
            }
            return userMaiData;
        }
    }
}
