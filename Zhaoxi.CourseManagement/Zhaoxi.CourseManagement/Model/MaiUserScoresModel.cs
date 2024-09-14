using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.CourseManagement.Model
{
    public class MaiUserScoresModel
    {
        public class Charts
        {
            public List<Dx> dx { get; set; }
            public List<Sd> sd { get; set; }
        }

        public class Dx
        {
            public int Zindex { get; set; }
            public int id { get; set; }
            public double achievements { get; set; }
            public double ds { get; set; }
            public int dxScore { get; set; }
            public int maxDxScore { get; set; }
            public string dx_max_str { get; set; }
            public string dx_src {  get; set; }
            public string fc { get; set; }
            public string fc_src { get; set; }
            public string fs { get; set; }
            public string fs_src { get; set; }
            public string level { get; set; }
            public int level_index { get; set; }
            public string level_label { get; set; }
            public int ra { get; set; }
            public string rate { get; set; }
            public string rate_src { get; set; }
            public int song_id { get; set; }
            public string song_img_src { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public string type_src { get; set; }
        }

        public class Root
        {
            public int additional_rating { get; set; }
            public Charts charts { get; set; }
            public string nickname { get; set; }
            public string plate { get; set; }
            public int rating { get; set; }
            public object user_general_data { get; set; }
            public string username { get; set; }
        }

        public class Sd
        {
            public int id { get; set; }
            public double achievements { get; set; }
            public double ds { get; set; }
            public int dxScore { get; set; }
            public string dx_max_str { get; set; }
            public int maxDxScore { get; set; }
            public string dx_src { get; set; }
            public string fc { get; set; }
            public string fc_src { get; set; }
            public string fs { get; set; }
            public string fs_src { get; set; }
            public string level { get; set; }
            public int level_index { get; set; }
            public string level_label { get; set; }
            public int ra { get; set; }
            public string rate { get; set; }
            public string rate_src { get; set; }
            public int song_id { get; set; }
            public string song_img_src { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public string type_src { get; set; }
        }


    }
}
