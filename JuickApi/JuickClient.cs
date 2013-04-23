using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using JuickApi.Common;
using Newtonsoft.Json;

namespace JuickApi {
    public class JuickClient : IJuickClient {
        static AuthenticationHeaderValue CreateBasicAuthorizationHeader(NetworkCredential credential) {
            var byteArray = Encoding.UTF8.GetBytes(credential.UserName + ":" + credential.Password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        readonly HttpClient client = new HttpClient();

        public JuickClient(NetworkCredential credential) {
            client.BaseAddress = new Uri("http://api.juick.com", UriKind.Absolute);
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthorizationHeader(credential);
        }

        #region IJuickClient Members

        public async Task<Message[]> GetFeed() {
            using(var response = await client.GetAsync("home?1=1")) {
                if(!response.IsSuccessStatusCode) {
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Message[]>(content);
                ForEach(result, FixEscaping);
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

        static void ForEach<T>(IEnumerable<T> enumerable, Action<T> action) {
            foreach(T item in enumerable) {
                action(item);
            }
        }

        static void FixEscaping(Message message) {
            if(message.Body.Contains("&quot;")) {
            }
            message.Body = HttpEncoder.HtmlDecode(message.Body);
        }
    }
}
