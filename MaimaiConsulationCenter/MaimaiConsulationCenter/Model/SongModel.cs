using System.Collections.Generic;

namespace MaimaiConsulationCenter.Model
{
    public class SongModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class BasicInfo
        {
            public string title { get; set; }
            public string artist { get; set; }
            /// <summary>
            /// 所属类别
            /// </summary>
            public string genre { get; set; }
            public int bpm { get; set; }
            public string release_date { get; set; }
            /// <summary>
            /// 所属版本
            /// </summary>
            public string from { get; set; }
            public bool is_new { get; set; }
        }

        public class Chart
        {
            public List<int> notes { get; set; }
            public string charter { get; set; }
            public double achivements { get; set; } = 0.0;
            public string fc { get; set; }
            public string fs { get; set; }
            public string rate { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string song_img_src { get; set; }
            public string title { get; set; } = "test";
            public string type { get; set; }
            /// <summary>
            /// 包含小数的定数
            /// </summary>
            public List<double> ds { get; set; }
            /// <summary>
            /// 仅包含是否+的定数
            /// </summary>
            public List<string> level { get; set; }
            public string easy { get; set; } = "";
            public string advanced { get; set; } = "";
            public string hard { get; set; } = "";
            public string master { get; set; } = "";
            public string remaster { get; set; } = "";
            public List<int> cids { get; set; }
            public List<Chart> charts { get; set; }
            public BasicInfo basic_info { get; set; }
        }

    }
}
