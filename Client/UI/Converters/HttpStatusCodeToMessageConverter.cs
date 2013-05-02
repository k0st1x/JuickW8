using System;
using System.Net;
using Windows.UI.Xaml.Data;

namespace Juick.Client.UI.Converters {
    public class HttpStatusCodeToMessageConverter : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(!(value is HttpStatusCode)) {
                return string.Empty;
            }
            var code = (HttpStatusCode)value;
            return string.Concat((int)code, ": ", code);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotSupportedException();
        }
        #endregion
    }
}
