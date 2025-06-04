using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace NTFSChecker.AvaloniaUI.Converters;

public class HexToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string hexColor)
            if (Color.TryParse(hexColor, out var color))
                return new SolidColorBrush(color);

        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
            return $"#{brush.Color.A:X2}{brush.Color.R:X2}{brush.Color.G:X2}{brush.Color.B:X2}";
        return "#00000000";
    }
}