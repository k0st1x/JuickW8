using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Client.Data;
using Juick.Common;
using Juick.Shared;

namespace Juick.Client.Services {
    public class MessagesSourceService : IMessagesSourceService {
        static string TagsToSingleString(string[] tags) {
            if(tags == null || tags.Length == 0) {
                return string.Empty;
            }
            return '*' + string.Join(" *", tags);
        }

        readonly IDictionary<GroupKind, Message[]> messagesByGroupKind = new Dictionary<GroupKind, Message[]>();
        readonly IDictionary<int, Reply[]> repliesByMId = new Dictionary<int, Reply[]>();
        readonly IJuickClient client;
        readonly ILocalStorageService localStorageService;

        public MessagesSourceService(IJuickClient client, ILocalStorageService localStorageService) {
            this.client = client;
            this.localStorageService = localStorageService;
        }

        #region IMessagesSourceService Members
        public async Task FillItems(SampleDataGroup group) {
            var groupKind = group.GroupKind;
            try {
                var localMessages = await localStorageService.LoadGroup(groupKind);
                group.Items.Clear();
                foreach(var message in localMessages) {
                    group.Items.Add(CreateItem(message, group));
                }

                var networkMessages = await GetClientGroupMessages(groupKind);
                group.Items.Clear();
                foreach(var message in networkMessages) {
                    group.Items.Add(CreateItem(message, group));
                }
                messagesByGroupKind[groupKind] = networkMessages;
                if(networkMessages != null) {
                    group.Items.Clear();
                    networkMessages
                        .Select(x => CreateItem(x, group))
                        .ForEach(group.Items.Add);
                }
            } catch {
                // todo: show message that working offline
            }
        }

        public async Task SaveState() {
            foreach(var groupPair in messagesByGroupKind.Where(x => x.Value != null)) {
                await localStorageService.SaveGroup(groupPair.Key, groupPair.Value);
            }
            foreach(var replyByMId in repliesByMId) {
                await localStorageService.SaveReplies(replyByMId.Key, replyByMId.Value);
            }
        }
        #endregion

        SampleDataItem CreateItem(Message message, SampleDataGroup group) {
            var photoUrl = message.Photo != null
                ? message.Photo.Medium
                : null;
            var userName = message.User != null ? message.User.UName : string.Empty;
            var item = new SampleDataItem(message.MId, userName, message.TimeStamp.ToString(), client.GetAvatarUrl(message.User), message.Body, TagsToSingleString(message.Tags), photoUrl, group);
            item.CommentsRequested += item_CommentsRequested;
            return item;
        }

        async void item_CommentsRequested(object sender, EventArgs e) {
            var item = (SampleDataItem)sender;
            item.CommentsRequested -= item_CommentsRequested;

            item.Replies.Add(item.ToTopReplyItem());

            var localReplies = await localStorageService.LoadReplies(item.MId);
            if(localReplies != null) {
                ClearTail(item.Replies);
                foreach(var localReply in localReplies) {
                    item.Replies.Add(CreateSampleDataReplyItem(item, localReply, null));
                }
            }
            try {
                var replies = await client.GetReplies(item.MId);
                if(replies != null) {
                    ClearTail(item.Replies);
                    foreach(var reply in replies) {
                        var photoUrl = reply.Photo != null
                            ? reply.Photo.Medium
                            : null;
                        var commentItem = CreateSampleDataReplyItem(item, reply, photoUrl);
                        item.Replies.Add(commentItem);
                    }
                }
            } catch {
                // todo: show error message
            }
        }

        Task<Message[]> GetClientGroupMessages(GroupKind groupKind) {
            switch(groupKind) {
                case GroupKind.None:
                    var tcs = new TaskCompletionSource<Message[]>();
                    tcs.SetResult(new Message[0]);
                    return tcs.Task;

                case GroupKind.MyFeed:
                    return client.GetMyFeed();

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

        SampleDataReplyItem CreateSampleDataReplyItem(SampleDataItem item, Reply reply, string photoUrl) {
            return new SampleDataReplyItem(reply.RId, reply.User.UName, reply.TimeStamp.ToString(), client.GetAvatarUrl(reply.User), reply.Body, photoUrl, item);
        }

        void ClearTail<T>(Collection<T> observableCollection) {
            while(observableCollection.Count > 1) {
                observableCollection.RemoveAt(1);
            }
        }
    }
}
