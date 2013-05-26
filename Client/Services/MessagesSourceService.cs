using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Client.Data;
using Juick.Common;
using Juick.Shared;
using System;

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
            if(messages != null) {
                messages
                    .Select(x => new SampleDataItem(x.MId, x.User.UName, x.TimeStamp.ToString(), null, TagsToSingleString(x.Tags), x.Body, group))
                    .ForEach(group.Items.Add);
            }
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
                return await GetClientGroupMessages(groupKind);
            } catch {
            }
            return await localStorageService.LoadGroup(groupKind);
        }

        Task<Message[]> GetClientGroupMessages(GroupKind groupKind) {
            switch(groupKind) {
                case GroupKind.None:
                    var tcs = new TaskCompletionSource<Message[]>();
                    tcs.SetResult(new Message[0]);
                    return tcs.Task;

                case GroupKind.MyFeed:
                    return client.GetMyFeed();
                case GroupKind.Private:
                    return client.GetPrivate();
                case GroupKind.Discussions:
                    return client.GetDiscussions();
                case GroupKind.Recommended:
                    return client.GetRecommended();
 
                case GroupKind.AllMessages:
                    return client.GetAllMessages();
                case GroupKind.Popular:
                    return client.GetPopular();
                case GroupKind.WithMedia:
                    return client.GetWithMedia();

                default:
                    throw new ArgumentException("groupKind");
            }
        }
    }
}
