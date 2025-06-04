using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NTFSChecker.AvaloniaUI.Models;

public partial class AccessUserEntry : ObservableObject
{
    [ObservableProperty] private string displayColorHex;

    public AccessUserEntry(IEnumerable<string> fields)
    {
        Fields = fields.ToList();
    }

    public AccessUserEntry()
    {
    }

    public List<string> Fields { get; set; } = new();

    public List<string> FieldsWithoutName
    {
        get { return Fields.Where((item, index) => index != 1).ToList(); }
    }
}