using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Annotations;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.DataAccess.DataEntity;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Net;
using MaimaiConsulationCenter.Model;
using System.Collections.ObjectModel;
using System.IO;
using static MaimaiConsulationCenter.Model.MaiUserScoresModel;
using System.CodeDom;

namespace MaimaiConsulationCenter.DataAccess
{
    public class LocalDataAccess
    {
        private static LocalDataAccess instance;
        private LocalDataAccess() { }
        public static LocalDataAccess GetInstance()
        {
            return instance??(instance=new LocalDataAccess());
        }
/*        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter adapter;
        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }*/
/*        private bool DBConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            if (conn == null) 
                conn = new MySqlConnection("Server=localhost;Database=zx_data;User Id=root;Password=123456;");
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }*/

        public void CheckUserInfo(string userName, string pwd)
        {
            try {
                /* //Console.WriteLine("登陆成功");
                 string userSql = "select * from users where user_name=@user_name and password=@pwd and is_validation=1";
                 //string userSql = "select * from users";
                 comm = new MySqlCommand(userSql,conn);
                 comm.Parameters.AddWithValue("user_name", userName);
                 comm.Parameters.AddWithValue("pwd", MD5Provider.GetMD5STring(pwd + "@" + userName));
                 MySqlDataReader rd = comm.ExecuteReader();


                 if (!rd.Read())
                     throw new Exception("用户名或密码不正确");
                 if (rd[5].ToString()=="0")
                     throw new Exception("当前用户没有权限使用此平台！");

                 UserEntity userInfo = new UserEntity();
                 userInfo.UserName = rd[1].ToString();
                 userInfo.RealName = rd[2].ToString();
                 userInfo.Password = rd[3].ToString();
                 userInfo.Avatar = rd[7].ToString();
                 userInfo.Gender = rd[8].ToString()[0] - '0';*/

                string jsonFilePath = Path.Combine(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."), // 向上返回两级目录
                    @"Assets\MaiMusicData\MusicData.json"
                );

                //读入歌曲json并反序列化
                string jsonFile = System.IO.File.ReadAllText(jsonFilePath);
                ObservableCollection<SongModel.Root> songDatas = JsonConvert.DeserializeObject<ObservableCollection<SongModel.Root>>(jsonFile);
                var _songDatas = songDatas.ToList();
                _songDatas.Reverse();
                GlobalValues.SongsModel = new ObservableCollection<SongModel.Root>();
                foreach (var item in _songDatas)
                    GlobalValues.SongsModel.Add(item);

                if (pwd != "") //如果包含密码
                {
                    //进行水鱼登录
                    var client = new RestClient("https://www.diving-fish.com/api/maimaidxprober/login");
                    var request = new RestRequest("", RestSharp.Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    var body = $@"{{""username"": ""{userName}"", ""password"": ""{pwd}""}}";
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    //非200状态码时报错并显示错误信息
                    if (((int)response.StatusCode) != 200)
                    {
                        Dictionary<string,object> jsonObj = JsonConvert.DeserializeObject<Dictionary<string,object>>(response.Content);
                        Console.WriteLine(jsonObj);
                        int errCode = Convert.ToInt32(jsonObj["errcode"]);
                        string msg = Convert.ToString(jsonObj["message"]);
                        throw new Exception($"{msg}");
                    }

                    //这里已经登陆成功
                    UserEntity userInfo = new UserEntity();
                    // 获取 Cookie 信息
                    if (response.Cookies != null && response.Cookies.Count > 0)
                    {
                        foreach (Cookie cookie in response.Cookies)
                        {
                            userInfo.Cookie = cookie.Value;
                            Console.WriteLine($"Name: {cookie.Name}, Value: {cookie.Value}");
                        }
                    }

                    GetB50(userName);
                    Console.WriteLine("登录界面-b50玩家数据获取成功");

                    userInfo.UserName = userName;
                    userInfo.RealName = userName;
                    userInfo.Password = pwd;
                    userInfo.Avatar = "./Assets/Images/avatarakira.jpg";
                    userInfo.Gender = 1;
                    GlobalValues.UserInfo = userInfo;

                    //继续获取玩家所有成绩
                    client = new RestClient("https://www.diving-fish.com/api/maimaidxprober/player/records");
                    request = new RestRequest("", RestSharp.Method.Get);
                    request.AddHeader("Cookie", $"jwt_token={userInfo.Cookie}");
                    response = client.Execute(request);

                    //非200状态码时报错并显示错误信息
                    if (((int)response.StatusCode) != 200)
                    {
                        Dictionary<string, object> jsonObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
                        Console.WriteLine(response.Content);
                        string msg = Convert.ToString(jsonObj["message"]);
                        throw new Exception($"{msg}");
                    }

                    GlobalValues.UserRecords = JsonConvert.DeserializeObject<UserRecordsModel.Root>(response.Content);
                    Console.WriteLine("登录界面：玩家所有成绩获取成功");
                    //在此将成绩归入所有乐曲成绩
                    foreach(var item in GlobalValues.UserRecords.records)
                    {
                        var targetSong = GlobalValues.SongsModel.FirstOrDefault(song => song.id == item.song_id.ToString());
                        if (targetSong != null)
                        {
                            targetSong.charts[item.level_index].achivements = item.achievements;
                            targetSong.charts[item.level_index].fc = item.fc;
                            targetSong.charts[item.level_index].fs = item.fs;
                            targetSong.charts[item.level_index].rate = item.rate;
                        }
                    }

                    return;
                }
                else
                {
                    GetB50(userName);
                    Console.WriteLine("登录界面-b50玩家数据获取成功");
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private void GetB50(string userName)
        {
            var client = new RestClient("https://www.diving-fish.com/api/maimaidxprober/query/player");
            //client.Timeout = -1;
            var request = new RestRequest("", RestSharp.Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
                    " + "\n" +
                    @"    ""b50"": true,
                    " + "\n" +
                    $@"    ""username"": ""{userName}""
                    " + "\n" +
                                @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            GlobalValues.B50 = JsonConvert.DeserializeObject<Root>(response.Content);

            switch ((int)response.StatusCode)
            {
                case 200:
                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = userName;
                    userInfo.RealName = userName;
                    userInfo.Password = userName;
                    userInfo.Avatar = "./Assets/Images/avatarakira.jpg";
                    userInfo.Gender = 1;
                    GlobalValues.UserInfo = userInfo;
                    break;
                case 400:
                    throw new Exception("不存在该用户");
                case 403:
                    throw new Exception("用户设置隐私保护或未同意隐私协议");
                default:
                    throw new Exception("未知错误");
            }

            //在此计算底板
            GlobalValues.B15Floor = GlobalValues.B50.charts.dx.Last().ra;
            GlobalValues.B35Floor = GlobalValues.B50.charts.sd.Last().ra;
            return;
        }
    }
}
