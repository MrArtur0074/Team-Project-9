using System.Globalization;
using System.Windows.Data;

namespace Coswalt.Converters
{
    public class BoolToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool isExpanded = value is bool b && b;
            return isExpanded ? 180 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}