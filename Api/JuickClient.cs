using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Juick.Shared;
using Newtonsoft.Json;

namespace Juick.Api {
    public sealed class JuickClient : IJuickClient, IDisposable {
        static AuthenticationHeaderValue CreateBasicAuthorizationHeader(NetworkCredential credential) {
            var byteArray = Encoding.UTF8.GetBytes(credential.UserName + ":" + credential.Password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        static void FixEscaping(Message message) {
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
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthorizationHeader(credential);
        }

        public async Task<HttpStatusCode> CheckStatusCode() {
            using(var post = await client.PostAsync("post", new ByteArrayContent(new byte[0]))) {
                return post.StatusCode;
            }
        }

        public async Task<Message[]> GetFeed() {
            using(var response = await client.GetAsync("home?1=1")) {
                if(!response.IsSuccessStatusCode) {
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Message[]>(content);
                result.ForEach(FixEscaping);
                return result;
            }
        }

        public async Task<Message[]> GetLast() {
            var content = await client.GetStringAsync("messages?1=1");
            return null;
        }

        public async Task<Message[]> GetTop() {
            var content = await client.GetStringAsync("messages?1=1&popular=1");
            return null;
        }
        #endregion

        #region IDisposable Members
        public void Dispose() {
            client.Dispose();
        }
        #endregion
    }
}
