using System.Net;
using System.Threading.Tasks;

namespace Juick.Api {
    public interface IJuickClient {
        void SetCredential(NetworkCredential credential);
        Task<HttpStatusCode> CheckStatusCode();
        Task<Message[]> GetMyFeed();
        Task<Message[]> GetAllMessages();
        Task<Message[]> GetPopular();
        Task<Message[]> GetWithMedia();
        Task<Comment[]> GetComments(int mid);
        string GetAvatarUrl(User user);
    }
}
