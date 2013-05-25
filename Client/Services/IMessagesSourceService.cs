using System.Threading.Tasks;
using Juick.Client.Data;

namespace Juick.Client.Services {
    public interface IMessagesSourceService {
        Task FillItems(SampleDataGroup group);
        void SaveState();
    }
}
