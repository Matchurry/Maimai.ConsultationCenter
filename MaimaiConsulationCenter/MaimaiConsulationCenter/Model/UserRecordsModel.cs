using System.Collections.Generic;

namespace MaimaiConsulationCenter.Model
{
    public class UserRecordsModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Record
        {
            public double achievements { get; set; }
            public double ds { get; set; }
            public int dxScore { get; set; }
            public string fc { get; set; }
            public string fs { get; set; }
            public string level { get; set; }
            public int level_index { get; set; }
            public string level_label { get; set; }
            public int ra { get; set; }
            public string rate { get; set; }
            public int song_id { get; set; }
            public string title { get; set; }
            public string type { get; set; }
        }

        public class Root
        {
            public int additional_rating { get; set; }
            public string nickname { get; set; }
            public string plate { get; set; }
            public int rating { get; set; }
            public List<Record> records { get; set; }
            public string username { get; set; }
        }


    }
}
