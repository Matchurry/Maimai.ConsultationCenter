using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaimaiConsulationCenter.Model
{
    public class SongModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class BasicInfo
        {
            public string title { get; set; }
            public string artist { get; set; }
            public string genre { get; set; }
            public int bpm { get; set; }
            public string release_date { get; set; }
            public string from { get; set; }
            public bool is_new { get; set; }
        }

        public class Chart
        {
            public List<int> notes { get; set; }
            public string charter { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public List<double> ds { get; set; }
            public List<string> level { get; set; }
            public List<int> cids { get; set; }
            public List<Chart> charts { get; set; }
            public BasicInfo basic_info { get; set; }
        }

    }
}
