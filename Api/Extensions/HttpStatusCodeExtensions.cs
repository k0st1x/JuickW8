using System.Net;

namespace Juick.Api.Extensions {
    public static class HttpStatusCodeExtensions {
        public static bool IsSuccess(this HttpStatusCode code) {
            return code == HttpStatusCode.OK;
        }

        public static bool IsAuthenticated(this HttpStatusCode code) {
            return code == HttpStatusCode.BadRequest;
        }
    }
}
