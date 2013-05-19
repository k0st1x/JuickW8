namespace Juick.Common.Services {
    public interface INavigationManager {
        void OpenMain();
        void OpenLogin();
        void OpenRead(string uniqueId);
    }
}
