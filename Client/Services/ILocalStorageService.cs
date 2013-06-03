using System.Threading.Tasks;
using Juick.Api;
using Juick.Common;

namespace Juick.Client.Services {
    public interface ILocalStorageService {
        Task SaveGroup(GroupKind groupKind, Message[] messages);
        Task<Message[]> LoadGroup(GroupKind groupKind);
        Task SaveReplies(int mid, Reply[] replies);
        Task<Reply[]> LoadReplies(int mid);
    }
}
