using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NTFSChecker.Core.Models;

namespace NTFSChecker.AvaloniaUI.Models;

public class TreeItem : ExcelDataModel
{
    public TreeItem(ExcelDataModel model)
        : base(model.ServerName, model.DirName, model.AccessUsers, model.ChangesFlag)
    {
        AccessUserEntries = new ObservableCollection<AccessUserEntry>(
            model.AccessUsers.Select(entry => new AccessUserEntry(entry))
        );
    }

    public TreeItem(string remoteName, string path, List<List<string>> accessUsers, bool areChanges)
        : base(remoteName, path, accessUsers, areChanges)
    {
        AccessUserEntries = new ObservableCollection<AccessUserEntry>(
            accessUsers.Select(entry => new AccessUserEntry(entry))
        );
    }


    public ObservableCollection<TreeItem> Children { get; set; } = new();

    public ObservableCollection<AccessUserEntry> AccessUserEntries { get; set; }

    public bool IsSelected { get; set; }
    public bool IsExpanded { get; set; }

    public string DisplayName => $"{DirName}";

    public bool HasPermissionIssues => ChangesFlag;
    public string Icon => !HasPermissionIssues ? "✔" : "⚠";
}