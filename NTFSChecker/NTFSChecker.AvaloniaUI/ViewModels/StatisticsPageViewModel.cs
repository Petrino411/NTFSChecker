using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NTFSChecker.AvaloniaUI.Models;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;


namespace NTFSChecker.AvaloniaUI.ViewModels;

public partial class StatisticsPageViewModel : ViewModelBase
{
    private string? _lastProcessedRoot;
    private readonly IDirectoryChecker _directoryChecker;
    private readonly ISettingsService _settingsService;
    private readonly IUserGroupHelper _reGroupHelper;
    
    private ObservableCollection<TreeItem> _fullTree = [];

    [ObservableProperty]
    private ObservableCollection<TreeItem> fileSystemTree = new();

    [ObservableProperty]
    private TreeItem selectedItem;

    [ObservableProperty]
    private string filter;
    
    

    public StatisticsPageViewModel(IDirectoryChecker directoryChecker, ISettingsService settingsService, IUserGroupHelper reGroupHelper)
    {
        _directoryChecker = directoryChecker;
        _settingsService = settingsService;
        _reGroupHelper = reGroupHelper;
        Filter = string.Empty;
        
    }

    public void RefreshIfNeeded()
    {
        if (_directoryChecker.RootData is not { Count: > 0 })
            return;

        var currentRoot = _directoryChecker.RootData.FirstOrDefault()?.DirName;

        if (_lastProcessedRoot == currentRoot)
            return;
        
        _lastProcessedRoot = currentRoot;
        Refresh();
    }
    
    [RelayCommand]
    private void Refresh()
    {
        _fullTree.Clear();
        SelectedItem = null;
        _fullTree = BuildTreeFromRoot(_directoryChecker.RootData);
        FileSystemTree = new ObservableCollection<TreeItem>(_fullTree);
    }

public ObservableCollection<TreeItem> BuildTreeFromRoot(List<ExcelDataModel> flatList)
{
    if (flatList == null || flatList.Count == 0)
        return new ObservableCollection<TreeItem>();

    flatList = _reGroupHelper.SetDescriptionsAsync(flatList).Result;

    var rootModel = flatList[0];
    var rootPath = Path.GetFullPath(rootModel.DirName);

    var rootItem = new TreeItem(rootModel);
    rootItem.AccessUserEntries = AnalyzeAccessRights(
        rootModel.AccessUsers,
        new List<List<string>>(),
        true);

    var lookup = new Dictionary<string, TreeItem>(StringComparer.OrdinalIgnoreCase)
    {
        [rootPath] = rootItem
    };

    foreach (var model in flatList.Skip(1))
    {
        var fullPath = Path.GetFullPath(model.DirName);
        if (!fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
            continue;

        var relativePath = Path.GetRelativePath(rootPath, fullPath);
        var parts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        string currentPath = rootPath;
        TreeItem parent = rootItem;

        for (int i = 0; i < parts.Length; i++)
        {
            currentPath = Path.Combine(currentPath, parts[i]);

            if (!lookup.TryGetValue(currentPath, out var currentNode))
            {
                var match = flatList.FirstOrDefault(m =>
                    Path.GetFullPath(m.DirName).Equals(currentPath, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    currentNode = new TreeItem(match);
                    currentNode.AccessUserEntries = AnalyzeAccessRights(
                        match.AccessUsers,
                        parent.AccessUsers,
                        false);
                }
                else
                {
                    currentNode = new TreeItem("", currentPath, new(), false);
                }

                parent.Children.Add(currentNode);
                lookup[currentPath] = currentNode;
            }

            parent = currentNode;
        }
    }

    return new ObservableCollection<TreeItem> { rootItem };
}
    
    partial void OnFilterChanged(string value)
    {
        ApplyFilter(value);
    }
    
    private void ApplyFilter(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            FileSystemTree = new ObservableCollection<TreeItem>(_fullTree);
        }
        else
        {
            var filtered = new ObservableCollection<TreeItem>();

            foreach (var item in _fullTree)
            {
                var match = FilterTree(item, filter);
                if (match != null)
                    filtered.Add(match);
            }

            FileSystemTree = filtered;
        }
    }
    
    private TreeItem? FilterTree(TreeItem node, string filter)
    {
        var matchedChildren = node.Children
            .Select(child => FilterTree(child, filter))
            .Where(child => child != null)
            .ToList();

        bool isMatch = node.DisplayName.Contains(filter, StringComparison.OrdinalIgnoreCase);

        if (isMatch || matchedChildren.Count > 0)
        {
            var copy = new TreeItem(node.ServerName, node.DirName, node.AccessUsers, node.ChangesFlag)
            {
                Children = new ObservableCollection<TreeItem>(matchedChildren!),
                IsExpanded = true, 
                IsSelected = node.IsSelected,
                AccessUserEntries = node.AccessUserEntries, 
            };

            return copy;
        }

        SelectedItem = null;
        return null;
    }
    
    
    private ObservableCollection<AccessUserEntry> AnalyzeAccessRights(
        List<List<string>> accessUsers,
        List<List<string>> mainDirAccessUsers,
        bool isRootDirectory)
    {
        var result = new ObservableCollection<AccessUserEntry>();
        int[] equalsIndices = { 1, 2, 3 };

        foreach (var accessUser in accessUsers)
        {
            var entry = new AccessUserEntry { Fields = accessUser };
            if (isRootDirectory)
            {
                entry.DisplayColorHex = "#000000";
            }
            else
            {
                var isGroupInMainDir = mainDirAccessUsers.Any(mu => mu[1] == accessUser[1]);

                if (isGroupInMainDir)
                {
                    if (mainDirAccessUsers.Any(main =>
                            equalsIndices.All(idx => main[idx] == accessUser[idx])))
                    {
                        entry.DisplayColorHex = "#000000";
                    }
                    else if (mainDirAccessUsers.Any(main => main[3] == accessUser[3]))
                    {
                        entry.DisplayColorHex = _settingsService.GetString("AppSettings:RightsColor"); // отличия в правах
                    }
                    else
                    {
                        entry.DisplayColorHex = _settingsService.GetString("AppSettings:RightsColor"); // тип прав отличается
                    }
                }
                else
                {
                    entry.DisplayColorHex = _settingsService.GetString("AppSettings:MainDirColor"); // нет в корне
                }
            }

            result.Add(entry);
        }

        // Записи из корня, которых нет в дочернем
        foreach (var main in mainDirAccessUsers)
        {
            if (!accessUsers.Any(x => x.SequenceEqual(main)))
            {
                result.Add(new AccessUserEntry
                {
                    Fields = main,
                    DisplayColorHex = _settingsService.GetString("AppSettings:CurDirColor") // отсутствует в дочернем
                });
            }
        }

        return result;
    }
    
}