using GalaSoft.MvvmLight.Messaging;
using MaimaiConsulationCenter.Common;
using MaimaiConsulationCenter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Interactivity;
using System.Windows;
using System.Reflection;

namespace MaimaiConsulationCenter.ViewModel
{
    public class SongDifComparer : IComparer<SongModel.Root>
    {
        public int Compare(SongModel.Root x, SongModel.Root y)
        {
            return x.ds[3].CompareTo(y.ds[3]); //比较紫谱的难度
        }
    }
    public class ScrollViewAni : Behavior<ScrollViewer>
    {
        private static double start_offset_ver = 0;
        private static double start_offset_hor = 0;
        private static Point mousePos = new Point();
        private static bool is_drag = false;
        private ScrollViewer sc;
        private int ScrolSense = 1000;
        protected override void OnAttached()
        {
            base.OnAttached();
            sc = AssociatedObject as ScrollViewer;
            //AssociatedObject.MouseLeftButtonDown += MoveAni;
            AssociatedObject.MouseMove += Move;
            //AssociatedObject.MouseLeftButtonUp += MoveAniEnd;
        }
        private void MoveAni(object sender, MouseEventArgs e)
        {
            is_drag = true;
            var sc = sender as ScrollViewer;
            mousePos = e.GetPosition(AssociatedObject);
            start_offset_ver = sc.ContentVerticalOffset;
            start_offset_hor = sc.ContentHorizontalOffset;
        }
        private void Move(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(AssociatedObject);
            if (point.X < 50 || point.Y < 50 || point.X > AssociatedObject.ActualWidth-50 || point.Y > AssociatedObject.ActualHeight-50) return;
            sc.ScrollToHorizontalOffset(point.X / AssociatedObject.ActualWidth * ScrolSense);
            sc.ScrollToVerticalOffset(point.Y / AssociatedObject.ActualHeight * ScrolSense);

            /*           Point now_MousePos = new Point();
                       var sc = sender as ScrollViewer;
                       if (is_drag)
                       {
                           now_MousePos = e.GetPosition(AssociatedObject);
                           sc.ScrollToHorizontalOffset(start_offset_hor - now_MousePos.X + mousePos.X);
                           sc.ScrollToVerticalOffset(start_offset_ver - now_MousePos.Y + mousePos.Y);
                       }*/
        }
        private void MoveAniEnd(object sender, MouseEventArgs e)
        {
            is_drag = false;
        }
    }
    public class VerClickGrid : Behavior<Grid>
    {
        private List<SongModel.Root> calingList = new List<SongModel.Root>();
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDown += VerClick;
        }
        private void VerClick(object sender, MouseEventArgs e)
        {
            var obj = AssociatedObject.DataContext;
            if (obj is VersionModel item)
            {
                if (GlobalValues.VersionsScoreShow.Count!=0 && GlobalValues.VersionsScoreShow[0].basic_info.from == item.VersionName) return;
            }
            else return;
            GlobalValues.VersionsScoreShow.Clear();
            calingList.Clear();
            foreach(var rec in GlobalValues.SongsModel)
            {
                //Console.WriteLine(rec.basic_info.from+" "+item.VersionName);
                if (rec.basic_info.from == item.VersionName)
                    calingList.Add(rec);
            }
            calingList.Sort(new SongDifComparer());
            //Console.WriteLine(calingList.Count());
            int cnt = 0;
            foreach(var rec in calingList)
            {
                GlobalValues.VersionsScoreShow.Insert(0, rec);
            }
        }
    }
    public class VersionProgressViewModel
    {
        public ObservableCollection<SongModel.Root> VersionSongs
        {
            get { return GlobalValues.VersionsScoreShow; }
        }
        public ObservableCollection<VersionModel> Versions
        {
            get { return GlobalValues.Versions; }
        }
        public VersionProgressViewModel()
        {

        }
    }
}
