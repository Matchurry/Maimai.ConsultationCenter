using MaimaiConsulationCenter.DataAccess.DataEntity;
using MaimaiConsulationCenter.Model;
using System.Collections.ObjectModel;

namespace MaimaiConsulationCenter.Common
{
    public class GlobalValues
    {
        public static bool is_pwd { get; set; } = false;
        public static UserEntity UserInfo { get; set; }
        public static MaiUserScoresModel.Root B50 { get; set; }
        public static ObservableCollection<SongModel.Root> SongsModel { get; set; }
        public static int B15_UI_Id { get; set; } = 0;
        public static int B35_UI_Id { get; set; } = 0;
        public static SongModel.Root SingleSongShow { get; set; } = new SongModel.Root();
        public static UserRecordsModel.Root UserRecords { get; set; } = new UserRecordsModel.Root();
        public static bool is_first_lauch { get; set; } = true;
        public static int now_dif_index { get; set; } = 0;
        public static int next_dif_dic { get; set; } = 0;
        public static int B15Floor { get; set; } = 0;
        public static int B35Floor { get; set; } = 0;
        public static ObservableCollection<VersionModel> Versions { get; set; } = new ObservableCollection<VersionModel>();
    }
}
