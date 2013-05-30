using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Client.Data;
using Juick.Client.Services;
using Juick.Common;
using Juick.Common.UI;
using System;
using System.Diagnostics;

namespace Juick.Client.ViewModels {
    public class MainViewModel : BindableBase {
        static readonly GroupKind[] readGroups = {
                                                     GroupKind.MyFeed,
                                                     GroupKind.AllMessages,
                                                     GroupKind.Popular,
                                                     GroupKind.WithMedia,
                                                 };

        readonly SampleDataGroup loginGroup = new SampleDataGroup(GroupKind.Login, "Log In", null, null);
        readonly SampleDataGroup logoutGroup = new SampleDataGroup(GroupKind.Logout, "Log Out", null, null);
        readonly SampleDataGroup postGroup = new SampleDataGroup(GroupKind.Post, "Post", null, null);
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;

        public ObservableCollection<SampleDataGroup> Items { get; private set; }
        public ICommand ItemPressCommand { get; private set; }

        public MainViewModel(IJuickClient client, INavigationManager navigationManager) {
            this.client = client;
            this.navigationManager = navigationManager;
            Items = new ObservableCollection<SampleDataGroup>(new[] {
                new SampleDataGroup(GroupKind.MyFeed, "My Feed", null, null),
                new SampleDataGroup(GroupKind.AllMessages, "All Messages", null, null),
                new SampleDataGroup(GroupKind.Popular, "Popular", null, null),
                new SampleDataGroup(GroupKind.WithMedia, "With Media", null, null)
            });
            ItemPressCommand = new DelegateCommand<SampleDataGroup>(ItemPress);

            Initialize();
        }

        async void Initialize() {
            var status = await client.CheckStatusCode();
            InitializeCore(status.IsAuthenticated());
        }

        private void InitializeCore(bool initialized) {
            if(initialized) {
                Items.Remove(loginGroup);
                Items.Add(logoutGroup);
                Items.Add(postGroup);
            } else {
                Items.Remove(logoutGroup);
                Items.Remove(postGroup);
                Items.Add(loginGroup);
            }
        }

        void ItemPress(SampleDataGroup parameter) {
            switch(parameter.GroupKind) {
                case GroupKind.MyFeed:
                case GroupKind.AllMessages:
                case GroupKind.Popular:
                case GroupKind.WithMedia:
                    navigationManager.OpenReadThreads(parameter);
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
