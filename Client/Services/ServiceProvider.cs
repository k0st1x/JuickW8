using System;
using Microsoft.Practices.Unity;

namespace Juick.Client.Services {
    public class ServiceProvider : IServiceProvider {
        readonly IUnityContainer container;
        public ServiceProvider(IUnityContainer container) {
            this.container = container;
        }

        #region IServiceProvider Members
        public object GetService(Type serviceType) {
            return container.Resolve(serviceType);
        }
        #endregion
    }
}
