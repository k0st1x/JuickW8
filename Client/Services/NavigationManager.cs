using System;
using Juick.Common.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Juick.Client.Services {
    public class NavigationManager : INavigationManager {
        #region INavigationManager Members
        public void OpenItems() {
            Navigate<ItemsPage>("AllGroups");
        }
        public void OpenLogin() {
            Navigate<LoginPage>();
        }
        #endregion

        void Navigate<T>() {
            FrameActCore(frame => frame.Navigate(typeof(T)));
        }

        void Navigate<T>(object param) {
            FrameActCore(frame => frame.Navigate(typeof(T), param));
        }

        async void FrameActCore(Action<Frame> action) {
            var frame = (Frame)Window.Current.Content;
            await frame.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => action(frame));
        }
    }
}
