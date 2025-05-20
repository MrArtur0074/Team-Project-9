using System.Globalization;
using System.Windows.Data;

namespace Coswalt.ViewModels
{
    public class EnumExtensions : IValueConverter
    {
        public static Array GetValues(Type enumType) => Enum.GetValues(enumType);

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            return parameter is Type enumType ? Enum.GetValues(enumType) : null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is string strValue && targetType.IsEnum) {
                return Enum.Parse(targetType, strValue);
            }

            return value!;
        }
    }
}