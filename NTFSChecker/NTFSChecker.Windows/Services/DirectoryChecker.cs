using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using NTFSChecker.Windows;
using NTFSChecker.Windows.Extensions;
using NTFSChecker.Windows.Models;

namespace NTFSChecker.Windows.Services;

public class DirectoryChecker(ILogger<DirectoryChecker> logger, SettingsService settingsService)
{
    private readonly SettingsService _settingsService = settingsService;
    public Action<string> logAction;

    public Action<(int, int)> progressAction;

    public List<ExcelDataModel> RootData { get; set; } = [];

    public async Task CheckDirectoryAsync(
        string path,
        Func<string, Task> logAction,
        Func<int, int, Task> progressAction)
    {
        NetworkPathResolver.TryGetRemoteComputerName(path, out string remoteComputerName);

        var rootAcl = await GetAccessRules(path);

        var dirs = 0;
        var files = 0;

        async Task CheckDirectoryAsyncInternal(string subPath)
        {
            var currentAcl = await GetAccessRules(subPath);
            if (currentAcl is not null)
            {
                if (!await CompareAccessRules(rootAcl, currentAcl))
                {
                    RootData.Add(new ExcelDataModel(remoteComputerName, subPath, currentAcl, true));
                    await logAction($"Различия в правах доступа обнаружены в папке: {subPath}");
                }
                else
                {
                    RootData.Add(new ExcelDataModel(remoteComputerName, subPath, currentAcl, false));
                }

                dirs++;
                await progressAction(dirs, files);
            }
            else
            {
                await logAction($"Ошибка при получении прав: {subPath}");
            }

            try
            {
                foreach (var directory in Directory.GetDirectories(subPath))
                {
                    await CheckDirectoryAsyncInternal(directory);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                await logAction($"Отказано в доступе: {subPath}\n {ex.Message}");
            }
            catch (Exception e)
            {
                await logAction($"Ошибка при получении прав: {subPath}");
            }

            try
            {
                foreach (var file in Directory.GetFiles(subPath))
                {
                    var fileAcl = await GetFileAccessRules(file);
                    if (fileAcl is not null)
                    {
                        if (!await CompareAccessRules(rootAcl, fileAcl))
                        {
                            RootData.Add(new ExcelDataModel(remoteComputerName, file, fileAcl, true));
                            await logAction($"Различия в правах доступа обнаружены в файле: {file}");
                        }
                        else
                        {
                            RootData.Add(new ExcelDataModel(remoteComputerName, file, fileAcl, false));
                        }

                        files++;
                        await progressAction(dirs, files);
                    }
                    else
                    {
                        await logAction($"Ошибка при получении прав: {subPath}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                await logAction($"Отказано в доступе: {subPath}\n {ex.Message}");
            }
            catch (Exception e)
            {
                await logAction($"Ошибка при получении прав: {subPath}\n  {e.Message}");
            }
        }

        await CheckDirectoryAsyncInternal(path);
    }

    private static async Task<bool> CompareAccessRules(IEnumerable<FileSystemAccessRule> acl1,
        IEnumerable<FileSystemAccessRule> acl2)
    {
        return await Task.Run(() =>
        {
            var acl1List = acl1.ToList();
            var acl2List = acl2.ToList();

            if (acl1List.Count != acl2List.Count)
            {
                return false;
            }

            foreach (var rule in acl1List)
            {
                var match = acl2List.Any(rule2 =>
                    rule.IdentityReference.Value == rule2.IdentityReference.Value &&
                    rule.AccessControlType == rule2.AccessControlType &&
                    rule.FileSystemRights == rule2.FileSystemRights);

                if (!match)
                {
                    return false;
                }
            }

            return true;
        });
    }

    private async Task<IEnumerable<FileSystemAccessRule>> GetAccessRules(string path)
    {
        return await Task.Run(() =>
        {
            var directoryInfo = new DirectoryInfo(path);
            try
            {
                var directorySecurity = directoryInfo.GetAccessControl();
                var acl = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                return FilterAccessRules(acl).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при получении прав доступа к файлу: {path}", path);
                return null;
            }
        });
    }

    private async Task<IEnumerable<FileSystemAccessRule>> GetFileAccessRules(string filePath)
    {
        return await Task.Run(() =>
        {
            var fileInfo = new FileInfo(filePath);
            try
            {
                var fileSecurity = fileInfo.GetAccessControl();
                var acl = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
                return FilterAccessRules(acl).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при получении прав доступа к файлу: {filePath}", filePath);
                return null;
            }
        });
    }

    private IEnumerable<FileSystemAccessRule> FilterAccessRules(AuthorizationRuleCollection acl)
    {
        bool.TryParse(_settingsService.GetString("AppSettings:IgnoreUndefined"),
            out var flag);

        foreach (FileSystemAccessRule rule in acl)
        {
            if (flag)
            {
                if (int.TryParse(rule.FileSystemRights.ToString(), out _))
                {
                    continue;
                }
            }

            yield return rule;
        }
    }

    private async Task LogToUIAsync(string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => logAction(message));
    }

    private async Task UpdateProgressAsync(int dirs, int files)
    {
        await Dispatcher.UIThread.InvokeAsync(() => progressAction((dirs, files)));
    }
}