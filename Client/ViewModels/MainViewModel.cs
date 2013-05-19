using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Client.Data;
using Juick.Common.Services;
using Juick.Common.UI;

namespace Juick.Client.ViewModels {
    public class MainViewModel : BindableBase {
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;

        public ObservableCollection<SampleDataGroup> Items { get; private set; }
        public ICommand ItemPressCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public MainViewModel(IJuickClient client, INavigationManager navigationManager) {
            this.client = client;
            this.navigationManager = navigationManager;
            Items = new ObservableCollection<SampleDataGroup>(new[] {
                new SampleDataGroup("read:my", "My Feed", "subtitle", null, "description"),
                new SampleDataGroup("read:private", "Private", "subtitle", null, "description"),
                new SampleDataGroup("read:discuss", "Discussions", "subtitle", null, "description"),
                new SampleDataGroup("read:recommended", "Recommended", "subtitle", null, "description"),
                new SampleDataGroup("read:xxx", "All Messages", "subtitle", null, "description"),
                new SampleDataGroup("read:top", "Popular", "subtitle", null, "description"),
                new SampleDataGroup("read:photos", "With photos", "subtitle", null, "description"),
                new SampleDataGroup("act:post", "Post", "subtitle", null, "description"),
                //new SampleDataGroup("act:logout", "Logout", "subtitle", null, "description"),
            });
            LogoutCommand = new DelegateCommand(Logout);
            ItemPressCommand = new DelegateCommand<SampleDataGroup>(ItemPress);

            Initialize();
        }

        async void Initialize() {
            var status = await client.CheckStatusCode();
            if(!status.IsAuthenticated()) {
                navigationManager.OpenLogin();
            } 
            //else {
                //foreach(var message in await client.GetFeed()) {
                //    var item = new SampleDataItem(message.MId.ToString(), message.User.UName, null, null, null, message.Body, messages);
                //    messages.Items.Add(item);
                //}
            //}
        }

        void ItemPress(SampleDataGroup parameter) {
            navigationManager.OpenRead(parameter.UniqueId);
        }

        void Logout() {
            navigationManager.OpenLogin();
        }
    }
}
