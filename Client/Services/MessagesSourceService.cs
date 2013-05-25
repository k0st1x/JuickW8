using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Client.Data;
using Juick.Common;
using Juick.Shared;

namespace Juick.Client.Services {
    public class MessagesSourceService : IMessagesSourceService {
        readonly IDictionary<GroupKind, Message[]> messagesByGroupKind = new Dictionary<GroupKind, Message[]>();
        readonly IJuickClient client;
        readonly ILocalStorageService localStorageService;

        public MessagesSourceService(IJuickClient client, ILocalStorageService localStorageService) {
            this.client = client;
            this.localStorageService = localStorageService;
        }

        #region IMessagesSourceService Members
        public async Task FillItems(SampleDataGroup group) {
            var groupKind = group.GroupKind;
            var messages = await GetGroupMessages(groupKind);
            messagesByGroupKind[groupKind] = messages;
            messages
                .Select(x => new SampleDataItem(x.MId, x.User.UName, x.TimeStamp.ToString(), null, TagsToSingleString(x.Tags), x.Body, group))
                .ForEach(group.Items.Add);
        }

        static string TagsToSingleString(string[] tags) {
            if(tags == null) {
                return string.Empty;
            }
            return '*' + string.Join(" *", tags);
        }

        public void SaveState() {
            foreach(var groupPair in messagesByGroupKind) {
                localStorageService.SaveGroup(groupPair.Key, groupPair.Value);
            }
        }
        #endregion

        async Task<Message[]> GetGroupMessages(GroupKind groupKind) {
            try {
                return await client.GetFeed();
            } catch {
            }
            return await localStorageService.LoadGroup(groupKind);
        }
    }
}
