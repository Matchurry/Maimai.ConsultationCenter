using LiveCharts;
using System.Collections.ObjectModel;

namespace MaimaiConsulationCenter.Model
{
    public class CourseSeriesModel
    {
        public string CourseName { get; set; }
        public SeriesCollection SC { get; set; }
        public ObservableCollection<SeriesModel> SeriesList { get; set; }
    }
}
