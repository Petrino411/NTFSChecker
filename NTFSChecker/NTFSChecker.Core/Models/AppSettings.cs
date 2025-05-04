using CommunityToolkit.Mvvm.ComponentModel;

namespace NTFSChecker.Core.Models;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty] private string mainDirColor = "";
    [ObservableProperty] private string curDirColor = "";
    [ObservableProperty] private string rightsColor = "";
    [ObservableProperty] private string defaultLDAPPath = string.Empty;
    [ObservableProperty] private bool ignoreUndefined = false;
    [ObservableProperty] private bool exportAll = false;
}