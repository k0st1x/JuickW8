using Juick.Client.ViewModels;

namespace Juick.Client {
    public sealed partial class MainPage : Juick.Client.Common.LayoutAwarePage {
        public MainPage() {
            this.InitializeComponent();
            AssignViewModel<MainViewModel>();
        }
    }
}
