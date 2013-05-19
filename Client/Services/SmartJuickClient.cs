using System.Net;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Common.Services;

namespace Juick.Client.Services {
    public class SmartJuickClient : JuickClient {
        public SmartJuickClient(ICredentialStorage credentialStorage) {
            var credential = credentialStorage.LoadCredential();
            if(credential != null) {
                SetCredential(credential);
            }
        }

        public override Task<HttpStatusCode> CheckStatusCode() {
            if(Credential == null) {
                var tcs = new TaskCompletionSource<HttpStatusCode>();
                tcs.SetResult(HttpStatusCode.Forbidden);
                return tcs.Task;
            }
            return base.CheckStatusCode();
        }
    }
}
