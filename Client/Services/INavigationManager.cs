using Juick.Common;

namespace Juick.Client.Services {
    public interface INavigationManager {
        void OpenMain();
        void OpenLogin();
        void OpenReadThreads(GroupKind kind);
    }
}
