using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Juick.Api {
    public interface IJuickClient {
        void SetCredential(NetworkCredential credential);
        Task<HttpStatusCode> CheckStatusCode();
        Task<Message[]> GetFeed();
        Task<Message[]> GetLast();
        Task<Message[]> GetTop();
    }
}
