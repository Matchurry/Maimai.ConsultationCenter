using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
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
using MaimaiConsulationCenter.Model;
using MaimaiConsulationCenter.ViewModel;
using static MaimaiConsulationCenter.Common.Interfaces;
using static MaimaiConsulationCenter.Model.MaiUserScoresModel;

namespace MaimaiConsulationCenter.View
{
    /// <summary>
    /// PointsSearchView.xaml 的交互逻辑
    /// </summary>
    public partial class PointsSearchView : UserControl, IDataLoadable
    {
        public PointsSearchView()
        {
            InitializeComponent();
            //Console.WriteLine("loaded");
        }
        public async Task InitializeDataAsync()
        {
            var DataContext = await new ScorePageViewModel().GetScorePageDataAsync();
            this.DataContext = DataContext;
        }
    }
}
