using System;
using Juick.Api;
using Juick.Client.Services;
using Juick.Common.Services;
using Microsoft.Practices.Unity;

namespace Juick.Client {
    public static class Bootstrapper {
        public static IUnityContainer CreateContainer() {
            var container = new UnityContainer();
            container.RegisterType<IJuickClient, SmartJuickClient>(new ContainerControlledLifetimeManager());
            container.RegisterType<IServiceProvider, UnityServiceProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICredentialStorage, CredentialStorage>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationManager, NavigationManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessagesSourceService, MessagesSourceService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocalStorageService, LocalStorageService>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}
