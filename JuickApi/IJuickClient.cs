using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuickApi {
    public interface IJuickClient {
        Task<Message[]> GetFeed();
        Task<Message[]> GetLast();
        Task<Message[]> GetTop();
    }
}
