using System.Collections.ObjectModel;
using System.Windows.Input;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Client.Data;
using Juick.Client.Services;
using Juick.Common;
using Juick.Common.UI;

namespace Juick.Client.ViewModels {
    public class MainViewModel : BindableBase {
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;
        bool isLoggedIn;

        public ObservableCollection<SampleDataGroup> Items { get; private set; }
        public ICommand ItemPressCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public bool IsLoggedIn {
            get { return isLoggedIn; }
        }

        public MainViewModel(IJuickClient client, INavigationManager navigationManager) {
            this.client = client;
            this.navigationManager = navigationManager;
            Items = new ObservableCollection<SampleDataGroup>(new[] {
                new SampleDataGroup(GroupKind.MyFeed, "My Feed", "subtitle", null, "description"),
                new SampleDataGroup(GroupKind.AllMessages, "All Messages", "subtitle", null, "description"),
                new SampleDataGroup(GroupKind.Popular, "Popular", "subtitle", null, "description"),
                new SampleDataGroup(GroupKind.WithMedia, "With Media", "subtitle", null, "description"),
                //new SampleDataGroup("act:post", "Post", "subtitle", null, "description"),
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
        }

        void ItemPress(SampleDataGroup parameter) {
            navigationManager.OpenReadThreads(parameter);
        }

        void Logout() {
            navigationManager.OpenLogin();
        }
    }
}
