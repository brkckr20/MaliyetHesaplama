using System.Globalization;
using System.Windows.Data;

namespace MaliyeHesaplama.helpers
{
    public class StringDotHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0d;

            string s = value.ToString().Replace(',', '.'); // Virgülü noktaya çevir
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;

            return 0d; // Geçersiz girişte 0
        }
    }
}
