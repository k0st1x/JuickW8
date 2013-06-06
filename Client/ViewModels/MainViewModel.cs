using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
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
        readonly IRootItemsContainer rootItemsContainer;
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;

        public ObservableCollection<SampleDataGroup> Items {
            get { return rootItemsContainer.Root; }
        }
        public ICommand ItemPressCommand { get; private set; }

        public MainViewModel(IRootItemsContainer rootItemsContainer, IJuickClient client, INavigationManager navigationManager) {
            this.rootItemsContainer = rootItemsContainer;
            this.client = client;
            this.navigationManager = navigationManager;
            ItemPressCommand = new DelegateCommand<SampleDataGroup>(ItemPress);

            Initialize();
        }

        async void Initialize() {
            var isAuthenticated = false;
            try {
                var status = await client.CheckStatusCode();
                isAuthenticated = status.IsAuthenticated();
            } catch(HttpRequestException) {
                isAuthenticated = false;
            }
            InitializeCore(isAuthenticated);
        }

        private void InitializeCore(bool initialized) {
            if(initialized) {
                rootItemsContainer.LoggedIn();
            } else {
                rootItemsContainer.LoggedOut();
            }
        }

        void ItemPress(SampleDataGroup parameter) {
            switch(parameter.GroupKind) {
                case GroupKind.MyFeed:
                case GroupKind.AllMessages:
                case GroupKind.Popular:
                case GroupKind.WithMedia:
                    navigationManager.OpenReadThreads(parameter.GroupKind);
                    return;

                case GroupKind.Logout:
                    InitializeCore(false);
                    return;

                case GroupKind.Login:
                    navigationManager.OpenLogin();
                    return;

                case GroupKind.Post:
                    throw new NotImplementedException();
            }
            Debug.Assert(false, parameter.GroupKind + " is not supported");
        }
    }
}
