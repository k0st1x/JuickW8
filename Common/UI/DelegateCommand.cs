using System;
using System.Windows.Input;

namespace Juick.Common.UI {
    public class DelegateCommand : DelegateCommand<object> {
        public DelegateCommand(Action action)
            : base(_ => action()) {
        }
    }
}
