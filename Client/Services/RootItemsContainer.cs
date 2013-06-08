using System.Collections.ObjectModel;
using Juick.Client.Data;
using Juick.Common;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Juick.Client.Services {
    public class RootItemsContainer : IRootItemsContainer {
        static Brush CreatesolidColorBrush(byte r, byte g, byte b) {
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        readonly ObservableCollection<SampleDataGroup> root = new ObservableCollection<SampleDataGroup>(new[] {
            new SampleDataGroup(GroupKind.MyFeed, "My Feed", "Assets/MyFeed250.png", "Assets/MyFeed60.png", CreatesolidColorBrush(26, 193, 235)),
            new SampleDataGroup(GroupKind.AllMessages, "All Messages", "Assets/AllMessages250.png", "Assets/AllMessages60.png", CreatesolidColorBrush(0, 159, 60)),
            new SampleDataGroup(GroupKind.Popular, "Popular", "Assets/Popular250.png", "Assets/Popular60.png", CreatesolidColorBrush(242, 208, 19)),
            new SampleDataGroup(GroupKind.WithMedia, "With Media", "Assets/WithMedia250.png", "Assets/WithMedia60.png", CreatesolidColorBrush(198, 0, 0))
        });
        readonly SampleDataGroup loginGroup = new SampleDataGroup(GroupKind.Login, "Log In", "Assets/LogIn250.png", "Assets/LogIn60.png", CreatesolidColorBrush(149, 23, 161));
        readonly SampleDataGroup logoutGroup = new SampleDataGroup(GroupKind.Logout, "Log Out", "Assets/LogOut250.png", "Assets/LogOut60.png", CreatesolidColorBrush(149, 23, 161));
        //readonly SampleDataGroup postGroup = new SampleDataGroup(GroupKind.Post, "Post", "Assets/Post250.png", "Assets/Post60.png", CreatesolidColorBrush(123, 179, 255));

        #region IRootItemsContainer Members
        public ObservableCollection<SampleDataGroup> Root {
            get { return root; }
        }

        public void LoggedIn() {
            if(root.Contains(logoutGroup)) {
                return;
            }
            root.Remove(loginGroup);
            root.Add(logoutGroup);
            //root.Add(postGroup);
        }

        public void LoggedOut() {
            if(root.Contains(loginGroup)) {
                return;
            }
            root.Remove(logoutGroup);
            //root.Remove(postGroup);
            root.Add(loginGroup);
        }
        #endregion
    }
}
