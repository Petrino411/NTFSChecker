using System;
using Avalonia.Media;

namespace NTFSChecker.Avalonia.Extentions;

public static class ColorExtentions
{
    public static Color FromHex(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Colors.Black;
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length == 6)
            hex = "FF" + hex; 

        if (hex.Length != 8)
            throw new FormatException("HEX-строка должна быть в формате #AARRGGBB или #RRGGBB.");

        var a = Convert.ToByte(hex.Substring(0, 2), 16);
        var r = Convert.ToByte(hex.Substring(2, 2), 16);
        var g = Convert.ToByte(hex.Substring(4, 2), 16);
        var b = Convert.ToByte(hex.Substring(6, 2), 16);

        return Color.FromArgb(a, r, g, b);
    }
    
    public static string ToHex(Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    
}