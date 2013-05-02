using System;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class NotConverter : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return Convert(value, targetType, parameter, language);
        }
        #endregion
    }
}
