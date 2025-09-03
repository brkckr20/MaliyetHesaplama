using System.Globalization;
using System.Windows.Data;

namespace MaliyeHesaplama.helpers
{
    public class StringDotHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return d.ToString("N2", CultureInfo.InvariantCulture);
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string ?? "0";
            s = s.Replace(',', '.'); // hem , hem . kabul et
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return 0;
        }
    }
}
