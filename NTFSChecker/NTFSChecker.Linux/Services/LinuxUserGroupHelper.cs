using Microsoft.Extensions.Logging;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;

namespace NTFSChecker.Linux.Services;

public class LinuxUserGroupHelper : IUserGroupHelper
{
    private readonly ILogger<IUserGroupHelper> _logger;

    // Кеш описаний
    private Dictionary<string, string> _userDescriptions = new();
    private Dictionary<string, string> _groupDescriptions = new();

    public LinuxUserGroupHelper(ILogger<IUserGroupHelper> logger)
    {
        _logger = logger;
        LoadUserDescriptions();
        LoadGroupDescriptions();
    }

    public void CheckDomainAvailability()
    {
        _logger.LogInformation("Проверка домена не поддерживается в Linux.");
    }

    public Task<List<ExcelDataModel>> SetDescriptionsAsync(List<ExcelDataModel> data)
    {
        foreach (var item in data)
        {
            foreach (var ac in item.AccessUsers)
            {
                var identity = ac[1].Split(':').Last(); 

                if (_userDescriptions.TryGetValue(identity, out var userDesc))
                {
                    ac[0] = userDesc;
                }
                else if (_groupDescriptions.TryGetValue(identity, out var groupDesc))
                {
                    ac[0] = groupDesc;
                }
                else
                {
                    continue;
                    ac[0] = "Нет описания";
                }
            }
        }

        return Task.FromResult(data);
    }

    private void LoadUserDescriptions()
    {
        try
        {
            var lines = File.ReadAllLines("/etc/passwd");
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length > 4)
                {
                    var username = parts[0];
                    var gecos = parts[4].Split(',')[0];
                    _userDescriptions[username] = string.IsNullOrWhiteSpace(gecos) ? "Нет описания" : gecos;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при чтении /etc/passwd: {ex.Message}");
        }
    }

    private void LoadGroupDescriptions()
    {
        try
        {
            var lines = File.ReadAllLines("/etc/group");
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length >= 1)
                {
                    var groupName = parts[0];
                    _groupDescriptions[groupName] = "Группа без описания"; 
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при чтении /etc/group: {ex.Message}");
        }
    }
}