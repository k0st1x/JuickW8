using System.Reflection;
using Juick.Client.Services;
using Juick.Client.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Controls;

namespace Juick.Client.Tests {
    [TestClass]
    public class BootstrapperTests {
        [TestMethod]
        public void ResolveAllTest() {
            using(var container = Bootstrapper.CreateContainer()) {
                foreach(var registration in container.Registrations) {
                    var item = container.Resolve(registration.RegisteredType);
                }
            }
        }

        [TestMethod]
        public void LoadPagesTest() {
            using(var container = Bootstrapper.CreateContainer()) {
                container.Resolve<LoginViewModel>();
            }
        }
    }
}
