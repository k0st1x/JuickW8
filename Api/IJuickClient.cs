using System.Collections.Generic;
using System.Threading.Tasks;

namespace Juick.Api {
    public interface IJuickClient {
        Task<Message[]> GetFeed();
        Task<Message[]> GetLast();
        Task<Message[]> GetTop();
    }
}
