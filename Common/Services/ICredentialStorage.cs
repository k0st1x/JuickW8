using System.Net;

namespace Juick.Common.Services {
    public interface ICredentialStorage {
        void SaveCredential(NetworkCredential credential);
        NetworkCredential LoadCredential();
    }
}
