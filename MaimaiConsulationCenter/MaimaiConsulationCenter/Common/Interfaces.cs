using System.Threading.Tasks;

namespace MaimaiConsulationCenter.Common
{
    public class Interfaces
    {
        public interface IDataLoadable
        {
            Task InitializeDataAsync();
        }
    }
}
