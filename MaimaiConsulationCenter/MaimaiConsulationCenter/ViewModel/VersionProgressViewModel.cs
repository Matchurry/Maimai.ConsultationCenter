using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace MaimaiConsulationCenter.ViewModel
{
    public class VersionProgressViewModel
    {
        public static ObservableCollection<SongModel.Root> test = new ObservableCollection<SongModel.Root>();
        public ObservableCollection<SongModel.Root> VersionSongs
        {
            get { return test; }
        }
        public ObservableCollection<VersionModel> Versions
        {
            get { return GlobalValues.Versions; }
        }
        public VersionProgressViewModel()
        {
            var item = new SongModel.Root();
            test.Insert(0,item);
            test.Insert(0,item);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            test.Insert(0, new SongModel.Root());
        }
    }
}
