using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class StringIsNullToVisibility : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            var stringValue = value as string;
            return string.IsNullOrEmpty(stringValue)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotSupportedException();
        }
        #endregion
    }
}
