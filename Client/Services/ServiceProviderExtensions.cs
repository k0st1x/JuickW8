using System;

namespace Juick.Client.Services {
    public static class ServiceProviderExtensions {
        public static T GetService<T>(this IServiceProvider serviceProvider) {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
