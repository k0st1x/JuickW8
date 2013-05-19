using System;
using System.Windows.Input;

namespace Juick.Common.UI {
    public class DelegateCommand<T> : ICommand {
        readonly Action<T> action;

        public DelegateCommand(Action<T> action) {
            this.action = action;
        }

        #region ICommand Members
        public bool CanExecute(object parameter) {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged {
            add { }
            remove { }
        }

        public void Execute(object parameter) {
            action((T)parameter);
        }
        #endregion
    }
}
