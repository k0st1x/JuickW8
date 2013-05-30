using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Juick.Shared;
using Newtonsoft.Json;

namespace Juick.Api {
    public class JuickClient : IJuickClient, IDisposable {
        static AuthenticationHeaderValue CreateBasicAuthorizationHeader(NetworkCredential credential) {
            var byteArray = Encoding.UTF8.GetBytes(credential.UserName + ":" + credential.Password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        static void FixEscaping(MessageBase message) {
            message.Body = HttpEncoder.HtmlDecode(message.Body);
        }

        readonly HttpClient client;

        public JuickClient() {
            client = new HttpClient {
                BaseAddress = new Uri("http://api.juick.com", UriKind.Absolute)
            };
        }

        #region IJuickClient Members
        public void SetCredential(NetworkCredential credential) {
            Credential = credential;
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthorizationHeader(credential);
        }

        public async virtual Task<HttpStatusCode> CheckStatusCode() {
            using(var post = await client.PostAsync("post", new ByteArrayContent(new byte[0]))) {
                return post.StatusCode;
            }
        }

        public Task<Message[]> GetMyFeed() {
            return ReadMessages("home?1=1");
        }

        public Task<Message[]> GetAllMessages() {
            return ReadMessages("messages?1=1");
        }

        public Task<Message[]> GetPopular() {
            return ReadMessages("messages?1=1&popular=1");
        }

        public Task<Message[]> GetWithMedia() {
            return ReadMessages("messages?1=1&media=all");
        }

        public Task<Reply[]> GetReplies(int mid) {
            return ReadMessages<Reply>("thread?mid=" + mid);
        }

        public string GetAvatarUrl(User user) {
            return new Uri(client.BaseAddress, "avatar?uname=" + user.UName).ToString();
        }
        #endregion

        #region IDisposable Members
        bool disposed;

        ~JuickClient() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(disposed) {
                return;
            }
            if(disposing) {
                client.Dispose();
            }
            disposed = true;
        }
        #endregion

        protected NetworkCredential Credential { get; private set; }

        async Task<T> ReadUri<T>(string url, Func<HttpContent, Task<T>> func) {
            using(var response = await client.GetAsync(url)) {
                return await (response.IsSuccessStatusCode
                    ? func(response.Content)
                    : func(null));
            }
        }

        Task<T[]> ReadMessages<T>(string url)
            where T : MessageBase {
            return ReadUri<T[]>(url, async content => {
                if(content == null) {
                    return null;
                }
                var contentString = await content.ReadAsStringAsync();
                var result = JsonConvert
                    .DeserializeObject<T[]>(contentString)
                    .Skip(1)
                    .ToArray();
                result.ForEach(x => FixEscaping(x));
                return result;
            });
        }

        async Task<Message[]> ReadMessages(string url) {
            var messages = await ReadMessages<Message>(url);
            messages.ForEach(FixTags);
            return messages;
        }

        static void FixTags(Message message) {
            if(message.Tags == null) {
                message.Tags = new string[0];
            }
        }
    }
}
