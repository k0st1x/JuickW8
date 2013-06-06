using System.Collections.ObjectModel;
using Juick.Client.Data;

namespace Juick.Client.Services {
    public interface IRootItemsContainer {
        ObservableCollection<SampleDataGroup> Root { get; }
        void LoggedIn();
        void LoggedOut();
    }
}
