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
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace Juick.Client.ViewModels {
    public class MainViewModel : BindableBase {
        static readonly GroupKind[] readGroups = {
                                                     GroupKind.MyFeed,
                                                     GroupKind.AllMessages,
                                                     GroupKind.Popular,
                                                     GroupKind.WithMedia,
                                                 };

        static Brush CreatesolidColorBrush(byte r, byte g, byte b) {
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        readonly SampleDataGroup loginGroup = new SampleDataGroup(GroupKind.Login, "Log In", "Assets/LogIn250.png", "Assets/LogIn60.png", CreatesolidColorBrush(149, 23, 161));
        readonly SampleDataGroup logoutGroup = new SampleDataGroup(GroupKind.Logout, "Log Out", "Assets/LogOut250.png", "Assets/LogOut60.png", CreatesolidColorBrush(149, 23, 161));
        readonly SampleDataGroup postGroup = new SampleDataGroup(GroupKind.Post, "Post", "Assets/Post250.png", "Assets/Post60.png", CreatesolidColorBrush(123, 179, 255));
        readonly IJuickClient client;
        readonly INavigationManager navigationManager;

        public ObservableCollection<SampleDataGroup> Items { get; private set; }
        public ICommand ItemPressCommand { get; private set; }

        public MainViewModel(IJuickClient client, INavigationManager navigationManager) {
            this.client = client;
            this.navigationManager = navigationManager;
            Items = new ObservableCollection<SampleDataGroup>(new[] {
                new SampleDataGroup(GroupKind.MyFeed, "My Feed", "Assets/MyFeed250.png", "Assets/MyFeed60.png", CreatesolidColorBrush(26, 193, 235)),
                new SampleDataGroup(GroupKind.AllMessages, "All Messages", "Assets/AllMessages250.png", "Assets/AllMessages60.png", CreatesolidColorBrush(0, 159, 60)),
                new SampleDataGroup(GroupKind.Popular, "Popular", "Assets/Popular250.png", "Assets/Popular60.png", CreatesolidColorBrush(242, 208, 19)),
                new SampleDataGroup(GroupKind.WithMedia, "With Media", "Assets/WithMedia250.png", "Assets/WithMedia60.png", CreatesolidColorBrush(198, 0, 0))
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
