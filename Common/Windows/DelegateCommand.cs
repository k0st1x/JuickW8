using System;
using System.Windows.Input;

namespace Juick.Common.Windows {
    public class DelegateCommand : ICommand {
        readonly Action action;

        public DelegateCommand(Action action) {
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
            action();
        }
        #endregion
    }
}
