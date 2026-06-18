using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using BankingSystem.Model;

namespace BankingSystem.Converters
{
    //              ==== Pomocna klasa koja konvertuje ENUM Country u sliku koja se printuje kao drzava
    public class CountryToFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Country country)
            {
                string code = country.ToString().ToLowerInvariant(); // e.g. "rs"
                try
                {
                    var uri = new Uri(
                        $"pack://application:,,,/Images/Flags/{code}.png",
                        UriKind.Absolute);
                    return new BitmapImage(uri);
                }
                catch
                {
                    return null; // missing flag image — just show nothing
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}