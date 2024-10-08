using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaimaiConsulationCenter.DataAccess.DataEntity;
using MaimaiConsulationCenter.Model;

namespace MaimaiConsulationCenter.Common
{
    public class GlobalValues
    {
        public static UserEntity UserInfo { get; set; }
        public static ObservableCollection<SongModel.Root> SongsModel { get; set; }
    }
}
