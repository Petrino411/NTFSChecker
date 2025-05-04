using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;

namespace NTFSChecker.Linux.Services;

public class LinuxDirectoryChecker : IDirectoryChecker
{
    private readonly ILogger<LinuxDirectoryChecker> _logger;
    private readonly ISettingsService _settingsService;
    private readonly INetworkPathResolver _networkPathResolver;

    public LinuxDirectoryChecker(ILogger<LinuxDirectoryChecker> logger, ISettingsService settingsService, INetworkPathResolver networkPathResolver)
    {
        _logger = logger;
        _settingsService = settingsService;
        _networkPathResolver = networkPathResolver;
    }

    public List<ExcelDataModel> RootData { get; set; } = new();

    public async Task CheckDirectoryAsync(string path, Func<string, Task> logAction, Func<int, int, Task> progressAction)
    {
        _networkPathResolver.TryGetRemoteComputerName(path, out var remoteComputerName);

        var rootAcl = GetAcl(path);

        int dirs = 0, files = 0;

        async Task Traverse(string subPath)
        {
            var currentAcl = GetAcl(subPath);

            if (currentAcl == null)
            {
                await logAction($"Ошибка при получении прав: {subPath}");
                return;
            }

            bool isDifferent = !CompareAcl(rootAcl, currentAcl);

            RootData.Add(new ExcelDataModel(remoteComputerName, subPath, FormatAclForExcel(currentAcl), isDifferent));

            if (isDifferent)
            {
                await logAction($"Различия в правах: {subPath}");
            }

            // await logAction(isDifferent
            //     ? $"Различия в правах: {subPath}"
            //     : $"Совпадают права: {subPath}");

            if (Directory.Exists(subPath))
            {
                dirs++;
                await progressAction(dirs, files);

                foreach (var dir in Directory.EnumerateDirectories(subPath))
                {
                    await Traverse(dir);
                }

                foreach (var file in Directory.EnumerateFiles(subPath))
                {
                    files++;
                    var fileAcl = GetAcl(file);
                    bool fileDiff = !CompareAcl(rootAcl, fileAcl);
                    RootData.Add(new ExcelDataModel(remoteComputerName, file, FormatAclForExcel(fileAcl), fileDiff));

                    await logAction(fileDiff
                        ? $"Различия в файле: {file}"
                        : $"Совпадают права в файле: {file}");

                    await progressAction(dirs, files);
                }
            }
        }

        await Traverse(path);
    }

    private List<string> GetAcl(string path)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "getfacl",
                Arguments = $"--absolute-names -p \"{path}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where(l => !l.StartsWith("#")) // Убираем метаинфо
                .ToList();

            return lines;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения ACL для: {Path}", path);
            return null;
        }
    }

    private bool CompareAcl(List<string> acl1, List<string> acl2)
    {
        if (acl1 == null || acl2 == null)
            return false;

        return Enumerable.SequenceEqual(acl1.OrderBy(x => x), acl2.OrderBy(x => x));
    }

    private List<List<string>> FormatAclForExcel(List<string> acl)
    {
        return acl.Select(line => new List<string>
        {
            line,        // raw acl entry
            "N/A",       // user
            "N/A",       // rights
            "N/A"        // access type
        }).ToList();
    }
}
