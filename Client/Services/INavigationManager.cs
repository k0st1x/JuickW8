using Juick.Client.Data;

namespace Juick.Client.Services {
    public interface INavigationManager {
        void OpenMain();
        void OpenLogin();
        void OpenReadThreads(SampleDataGroup group);
    }
}
