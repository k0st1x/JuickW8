using System.Threading.Tasks;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Common.Services;

namespace Juick.Client.ViewModels {
    public class ItemsViewModel : BindableBase {
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;

        public ItemsViewModel(IJuickClient client, INavigationManager navigationManager) {
            this.client = client;
            this.navigationManager = navigationManager;
            CheckConnection();
        }

        async void CheckConnection() {
            var status = await client.CheckStatusCode();
            if(!status.IsAuthenticated()) {
                navigationManager.OpenLogin();
            }
        }
    }
}
