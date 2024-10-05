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
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows;
using Zhaoxi.CourseManagement.Common;
using Zhaoxi.CourseManagement.Model;
using static System.Net.WebRequestMethods;
using static Zhaoxi.CourseManagement.Model.MaiUserScoresModel;
using static Zhaoxi.CourseManagement.ViewModel.LoginViewModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Mysqlx.Datatypes;

namespace Zhaoxi.CourseManagement.ViewModel
{
    public class ResizeByMouseBehavior : Behavior<FrameworkElement>
    {
        private WeakEventManager<Window, MouseEventArgs> _mouseMoveEventManager;
        protected override void OnAttached()
        {
            base.OnAttached();
            Application.Current.MainWindow.MouseMove += MouseMove;
        }

        protected override void OnDetaching()
        {
            Application.Current.MainWindow.MouseMove -= MouseMove;
            base.OnDetaching();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point mousePosition = AssociatedObject.PointToScreen(e.GetPosition(AssociatedObject)); //屏幕坐标系下鼠标的位置
                double y = mousePosition.Y - (AssociatedObject.PointToScreen(new Point(0, 0)).Y + 100 / 2); // y距离 有正负
                double x = mousePosition.X - (AssociatedObject.PointToScreen(new Point(0, 0)).X + 216 / 2); // x距离 有正负
                double distance = Math.Sqrt(x * x + y * y);
                double maxdis = Math.Sqrt(216 * 216 + 100 * 100) / 2;

                double tarsX=1, tarsY=1, tarX=0, tarY=0;
                //动画1 鼠标在控件内部时 控件放大
                if (Math.Abs(x) <= 216 / 2 && Math.Abs(y) <= 100 / 2) //鼠标在控件内 应用缩放
                {
                    //最大变为原来的1.3倍 线性
                    tarsX = 1.3 - distance / maxdis * 0.3;
                    tarsY = 1.3 - distance / maxdis * 0.3;
                }

                //动画2 对于同一行布局X大小的变换
                if (Math.Abs(y) <= 100 / 2) //如果鼠标在我这一行
                {
                    if(Math.Abs(x) >= 226/2  && Math.Abs(x) <= 206 + 226/2) //鼠标在我之外一个范围内
                    {
                        var cal = Math.Abs(x);
                        tarX = (cal-226/2)*20/106;
                        if (cal > 226) tarX = 40 - (cal - 226 / 2) * 20 / 106;
                        if (x>0) tarX = -tarX;
                    }
                }
                
                //动画3 对于同一列布局Y大小的变换
                if (Math.Abs(x) <= 216 / 2) //如果鼠标在我这一行
                {
                    if (Math.Abs(y) >= 110 / 2 && Math.Abs(y) <= 90 + 110 / 2) //鼠标在我之外一个范围内
                    {
                        var cal = Math.Abs(y);
                        tarY = (cal - 100 / 2) * 10 / 50;
                        if (cal > 95) tarY = 20 - (cal - 100 / 2) * 10 / 50;
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
            catch {Application.Current.MainWindow.MouseMove -= MouseMove; }

        }

    }
    public class ScorePageViewModel:NotifyBase
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
                $@"    ""username"": ""{LoginViewModel.LoginModel.UserName}""
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
            if(userMaiData.charts!=null && userMaiData.charts.dx.Count != 0)
            foreach (var item in userMaiData.charts.dx)
            {
                item.Zindex = 15 - cnt;
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
                item.id = cnt++;
                item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                item.type_src = String.Format("../Assets/Images/MaiType/{0}.png",item.type);
                item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                if (item.fs == "fsdp") item.fs_src = "../Assets/Images/MaiFcAp/fsd.png";
                item.animationlengh = string.Format("0:0:{0}.{1}",item.id/10+1,item.id%10);

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
            if(userMaiData.charts != null && userMaiData.charts.sd.Count!=0)
            foreach (var item in userMaiData.charts.sd)
            {
                item.song_img_src = String.Format("https://www.diving-fish.com/covers/{0:D5}.png", item.song_id);
                item.id = cnt++;
                item.rate_src = String.Format("../Assets/Images/MaiRanks/{0}.png", item.rate);
                item.type_src = String.Format("../Assets/Images/MaiType/{0}.png", item.type);
                item.fc_src = string.Format("../Assets/Images/MaiFcAp/{0}.png", item.fc);
                item.fs_src = string.Format("../Assets/Images/MaiFsFDX/{0}.png", item.fs);
                if (item.fs == "fdsp") item.fs_src = "../Assets/Images/MaiFcAp/fsd.png";
                item.animationlengh = string.Format("0:0:{0}.{1}", (item.id+15) / 10 + 1, (item.id+15) % 10);

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
