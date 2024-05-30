using System;
using System.IO;

namespace NTFSChecker.Extentions;

public static class PathExtention
{
    public static string baseDirectory { get; set; } = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;

    public static string applicationRoot { get; set; } = Directory.GetParent(Directory.GetParent(baseDirectory).FullName).FullName;
}