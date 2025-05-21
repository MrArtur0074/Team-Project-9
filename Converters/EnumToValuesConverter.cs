using System.Globalization;
using System.Windows.Data;

namespace Coswalt.Converters
{
    public class EnumToValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Type enumType && enumType.IsEnum)
                return Enum.GetValues(enumType);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}