using System;
using Juick.Api;
using Microsoft.Practices.Unity;

namespace Juick.Client.Services {
    static class Bootstrapper {
        public static IUnityContainer CreateContainer() {
            var container = new UnityContainer();
            var containerControlledLifetimeManager = new ContainerControlledLifetimeManager();
            container.RegisterType<IJuickClient, JuickClient>(containerControlledLifetimeManager);
            container.RegisterType<IServiceProvider, ServiceProvider>(containerControlledLifetimeManager);
            return container;
        }
    }
}
