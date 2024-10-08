using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;

namespace MaimaiConsulationCenter.ViewModel
{
    public class FirstPageViewModel:NotifyBase
    {
		private double _instrumentValue=0.0;

		public double InstrumentValue
		{
			get { return _instrumentValue; }
			set { _instrumentValue = value; DoNotify(); }
		}

        public ObservableCollection<CourseSeriesModel> CourseSeriesList { get; set; } = new ObservableCollection<CourseSeriesModel>();

        Random random = new Random();
        bool taskSwitch = true;
        List<Task> taskList = new List<Task>();
        public FirstPageViewModel()
        {
            RefreshInstrumentValue();
            InitCourseSeries();
        }

        private void InitCourseSeries()
        {
            CourseSeriesList.Add(new CourseSeriesModel
            {
                CourseName = "交互配置练习",
                SC = new LiveCharts.SeriesCollection { new PieSeries { 
                    Title = "轴交", 
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) }, 
                    DataLabels = false },new PieSeries {
                    Title = "大宇宙",
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) },
                    DataLabels = false }
                },
                SeriesList = new ObservableCollection<SeriesModel> 
                {
                    new SeriesModel{SeriesName="B站",CurrentValue=161,IsGrowing=true,ChangeRate=114},
                    new SeriesModel{SeriesName="云课堂",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="抖音",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="微博",CurrentValue=161,IsGrowing=true,ChangeRate=514},
                    new SeriesModel{SeriesName="想不出名字了",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                }
            });

            CourseSeriesList.Add(new CourseSeriesModel
            {
                CourseName = "大b扫键练习",
                SC = new LiveCharts.SeriesCollection { new PieSeries {
                    Title = "曲目1",
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) },
                    DataLabels = false },new PieSeries {
                    Title = "曲目2",
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) },
                    DataLabels = false }
                },
                SeriesList = new ObservableCollection<SeriesModel>
                {
                    new SeriesModel{SeriesName="文字1",CurrentValue=161,IsGrowing=true,ChangeRate=114},
                    new SeriesModel{SeriesName="文字2",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="文字3",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="文字4",CurrentValue=161,IsGrowing=true,ChangeRate=514},
                    new SeriesModel{SeriesName="文字5",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                }
            });

            CourseSeriesList.Add(new CourseSeriesModel
            {
                CourseName = "星星歌专场",
                SC = new LiveCharts.SeriesCollection { new PieSeries {
                    Title = "曲目1",
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) },
                    DataLabels = false },new PieSeries {
                    Title = "曲目2",
                    Values = new ChartValues<ObservableValue>{new ObservableValue(123) },
                    DataLabels = false }
                },
                SeriesList = new ObservableCollection<SeriesModel>
                {
                    new SeriesModel{SeriesName="文字1",CurrentValue=161,IsGrowing=true,ChangeRate=114},
                    new SeriesModel{SeriesName="文字2",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="文字3",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                    new SeriesModel{SeriesName="文字4",CurrentValue=161,IsGrowing=true,ChangeRate=514},
                    new SeriesModel{SeriesName="文字5",CurrentValue=161,IsGrowing=false,ChangeRate=-75},
                }
            });
        }

        private void RefreshInstrumentValue()
        {
            var task = Task.Factory.StartNew(new Action(async () => 
            {
                while (taskSwitch)
                {
                    InstrumentValue = random.Next((int)InstrumentValue-5, (int)InstrumentValue +5);
                    await Task.Delay(1000);
                }
            }));
            taskList.Add(task);
        }
        public void Dispose()
        {
            try { 
            taskSwitch = false;
            Task.WaitAll(taskList.ToArray());
            }
            catch
            {

            }
        }
    }
}
