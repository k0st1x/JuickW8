﻿using System;
using Juick.Client.Data;
using Juick.Common;
using Juick.Common.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Juick.Client.Services {
    public class NavigationManager : INavigationManager {
        Frame frame;

        Frame Frame {
            get { return frame ?? (frame = (Frame)Window.Current.Content); }
        }

        #region INavigationManager Members
        public void OpenMain() {
            Navigate<MainPage>();
        }
        
        public void OpenLogin() {
            Navigate<LoginPage>();
        }

        public void OpenReadThreads(SampleDataGroup group) {
            Navigate<ReadThreadsPage>(group);
        }
        #endregion

        void Navigate<T>() {
            FrameActCore(frame => frame.Navigate(typeof(T)));
        }

        void Navigate<T>(object param) {
            FrameActCore(frame => frame.Navigate(typeof(T), param));
        }

        async void FrameActCore(Action<Frame> action) {
            await Frame.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => action(Frame));
        }
    }
}
