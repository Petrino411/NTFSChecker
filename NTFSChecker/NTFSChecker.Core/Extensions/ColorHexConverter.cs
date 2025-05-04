using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace NTFSChecker.Core.Extensions;

public class ColorHexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Color.TryParse(value?.ToString(), out var color) ? color : Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color c)
            return c.ToString();
        return "#000000";
    }
}