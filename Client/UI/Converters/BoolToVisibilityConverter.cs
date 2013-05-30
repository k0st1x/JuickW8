using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class BoolToVisibilityConverter : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value == null
                ? Visibility.Collapsed
                : ConvertCore((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotSupportedException();
        }
        #endregion

        protected virtual Visibility ConvertCore(bool value) {
            return value
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
