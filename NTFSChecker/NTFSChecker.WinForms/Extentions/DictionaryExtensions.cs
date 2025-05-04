using System;
using System.Collections.Generic;

namespace NTFSChecker.WinForms.Extentions;

public static class DictionaryExtensions
{
    public static Dictionary<string, string> ToDictionaryWithSuffix(
        this IEnumerable<string[]> source,
        Func<string[], string> keySelector,
        Func<string[], string> elementSelector)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (var item in source)
        {
            var key = keySelector(item);
            var value = elementSelector(item);
            var originalKey = key;
            var suffix = 1;

            while (dictionary.ContainsKey(key))
            {
                key = $"{originalKey}_{suffix}";
                suffix++;
            }

            dictionary[key] = value;
        }

        return dictionary;
    }
}