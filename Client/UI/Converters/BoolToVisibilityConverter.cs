using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class BoolToVisibilityConverter : IValueConverter{
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            return (bool)value
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return (Visibility)value == Visibility.Visible;
        }   
        #endregion
    }
}
