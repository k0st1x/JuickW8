using System.Net;
using Juick.Api;

namespace Juick.Common.Services {
    public interface IJuickClientService {
        IJuickClient Client { get; }
        void SaveCredential(NetworkCredential credential);
        NetworkCredential LoadCredential();
    }
}
