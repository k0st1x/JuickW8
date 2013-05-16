using System;
using Juick.Api;
using Juick.Common.Services;
using Microsoft.Practices.Unity;

namespace Juick.Client.Services {
    static class Bootstrapper {
        public static IUnityContainer CreateContainer() {
            var container = new UnityContainer();
            container.RegisterType<IJuickClient, JuickClient>(new ContainerControlledLifetimeManager());
            container.RegisterType<IServiceProvider, ServiceProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICredentialStorage, CredentialStorage>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}
