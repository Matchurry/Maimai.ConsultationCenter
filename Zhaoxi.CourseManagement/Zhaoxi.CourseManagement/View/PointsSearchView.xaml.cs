using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zhaoxi.CourseManagement.ViewModel;

namespace Zhaoxi.CourseManagement.View
{
    /// <summary>
    /// PointsSearchView.xaml 的交互逻辑
    /// </summary>
    public partial class PointsSearchView : UserControl
    {
        public PointsSearchView()
        {
            InitializeComponent();
            DataContext = new CoursePageViewModel();

            var client = new RestClient("https://www.diving-fish.com/api/maimaidxprober/query/player");
            //client.Timeout = -1;
            var request = new RestRequest("",RestSharp.Method.Post);
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
            Console.WriteLine(response.Content);
        }
    }
}
