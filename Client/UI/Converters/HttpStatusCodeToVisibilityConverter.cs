using System;
using System.Net;
using Juick.Api.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class HttpStatusCodeToVisibilityConverter : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(!(value is HttpStatusCode)) {
                return Visibility.Collapsed;
            }
            var code = (HttpStatusCode)value;
            return code.IsSuccess()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotSupportedException();
        }
        #endregion
    }
}
