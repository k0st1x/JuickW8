using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Juick.Client.UI {
    public static class ListViewBaseItemClickCommand {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListViewBaseItemClickCommand), new PropertyMetadata(null, CommandPropertyChanged));

        public static void SetCommand(DependencyObject attached, ICommand value) {
            attached.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject attached) {
            return (ICommand)attached.GetValue(CommandProperty);
        }

        private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var listView = (ListViewBase)d;
            var command = (ICommand)e.NewValue;
            listView.ItemClick += (s, ev) => {
                var parameter = ev.ClickedItem;
                if(command.CanExecute(parameter)) {
                    command.Execute(parameter);
                }
            };
        }
    }
}
