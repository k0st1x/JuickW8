using System;
using System.Collections.Generic;
using System.Linq;
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
                    .Select(x => CreateItem(x, group))
                    .ForEach(group.Items.Add);
            }
        }

        public void SaveState() {
            foreach(var groupPair in messagesByGroupKind) {
                localStorageService.SaveGroup(groupPair.Key, groupPair.Value);
            }
        }
        #endregion

        SampleDataItem CreateItem(Message message, SampleDataGroup group) {
            var photoUrl = message.Photo != null
                ? message.Photo.Medium
                : null;
            var item = new SampleDataItem(message.MId, message.User.UName, message.TimeStamp.ToString(), client.GetAvatarUrl(message.User), message.Body, TagsToSingleString(message.Tags), photoUrl, group);
            item.CommentsRequested += item_CommentsRequested;
            return item;
        }

        async void item_CommentsRequested(object sender, EventArgs e) {
            var item = (SampleDataItem)sender;
            item.CommentsRequested -= item_CommentsRequested;

            item.Comments.Add(item.ToTopReplyItem());

            var replies = await client.GetReplies(item.MId);
            if(replies != null) {
                foreach(var reply in replies) {
                    var photoUrl = reply.Photo != null
                        ? reply.Photo.Medium
                        : null;
                    var commentItem = new SampleDataReplyItem(reply.RId, reply.User.UName, reply.TimeStamp.ToString(), client.GetAvatarUrl(reply.User), reply.Body, photoUrl, item);
                    item.Comments.Add(commentItem);
                }
            }
        }

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
