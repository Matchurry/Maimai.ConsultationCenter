using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.CourseManagement.Model;

namespace Zhaoxi.CourseManagement.ViewModel
{
    public class CoursePageViewModel
    {
        public ObservableCollection<CategoryItemModel> CategoryCourses { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryTechnology { get; set; }
        public ObservableCollection<CategoryItemModel> CategorysTeacher { get; set; }

        public CoursePageViewModel()
        {
            CategoryCourses = new ObservableCollection<CategoryItemModel>();
            CategoryCourses.Add(new CategoryItemModel("全部", true));
            CategoryCourses.Add(new CategoryItemModel("公开课"));
            CategoryCourses.Add(new CategoryItemModel("VIP课"));

            CategoryTechnology = new ObservableCollection<CategoryItemModel>();
            CategoryTechnology.Add(new CategoryItemModel("全部", true));
            CategoryTechnology.Add(new CategoryItemModel("C#"));
            CategoryTechnology.Add(new CategoryItemModel("ASP.NET"));
            CategoryTechnology.Add(new CategoryItemModel("JAVA"));
            CategoryTechnology.Add(new CategoryItemModel("微服务"));
            CategoryTechnology.Add(new CategoryItemModel("Vue"));
            CategoryTechnology.Add(new CategoryItemModel("微信小程序"));
            CategoryTechnology.Add(new CategoryItemModel("Winform"));
            CategoryTechnology.Add(new CategoryItemModel("WPF"));
            CategoryTechnology.Add(new CategoryItemModel("上位机"));

            CategorysTeacher = new ObservableCollection<CategoryItemModel>();
            CategorysTeacher.Add(new CategoryItemModel("全部", true));
            CategorysTeacher.Add(new CategoryItemModel("Eleven"));
            CategorysTeacher.Add(new CategoryItemModel("Richard"));
            CategorysTeacher.Add(new CategoryItemModel("Clay"));
            CategorysTeacher.Add(new CategoryItemModel("Garry"));
            CategorysTeacher.Add(new CategoryItemModel("Ace"));
            CategorysTeacher.Add(new CategoryItemModel("Leah"));
            CategorysTeacher.Add(new CategoryItemModel("Jovan"));
        }
    }
}
