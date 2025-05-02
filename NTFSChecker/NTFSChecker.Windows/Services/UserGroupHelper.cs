using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NTFSChecker.Windows;
using NTFSChecker.Windows.Extensions;
using NTFSChecker.Windows.Models;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;


namespace NTFSChecker.Windows.Services;

public class UserGroupHelper
{
    private Dictionary<string, string> LocalGroups { get; set; }
    private Dictionary<string, string> LocalUsers { get; set; }

    private bool _isDomainAvailable;


    private readonly ILogger<UserGroupHelper> _logger;
    private readonly SettingsService _settingsService;

    public UserGroupHelper(ILogger<UserGroupHelper> logger, SettingsService settingsService)
    {
        _logger = logger;
        _settingsService = settingsService;
        CheckDomainAvailability();
        LocalGroups = GetLocalUserGroupsAsync().Result;
        LocalUsers = GetLocalUsersAsync().Result;
    }

    public void CheckDomainAvailability()
    {
        var path = _settingsService.GetString("AppSettings:DefaultLDAPPath");
        try
        {
            using (var rootDSE = new DirectoryEntry(path))
            {
                _isDomainAvailable = rootDSE.Name != null;
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Domain {path} is not available.");
            _isDomainAvailable = false;
        }
    }

    public async Task<List<ExcelDataModel>> SetDescriptionsAsync(List<ExcelDataModel> data)
    {
        try
        {
            var datacopy = new List<ExcelDataModel>();
            foreach (var item in data)
            {
                datacopy.Add(await FillUserOrGroupDescriptionsAsync(item));
            }

            return datacopy;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка в получении описания: {ex.Message}? {data}");
        }

        return data;
    }


    private async Task<Dictionary<string, string>> GetLocalUserGroupsAsync()
    {
        var userGroups = new Dictionary<string, string>();
        try
        {
            using (var context = new PrincipalContext(ContextType.Machine))
            using (var searcher = new PrincipalSearcher(new GroupPrincipal(context)))
            {
                foreach (var result in searcher.FindAll())
                {
                    if (result is GroupPrincipal group)
                    {
                        userGroups[group.SamAccountName] = group.Description;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Проверка не пройдена: {ex.Message}");
        }

        return userGroups;
    }

    private async Task<Dictionary<string, string>> GetLocalUsersAsync()
    {
        var users = new Dictionary<string, string>();

        try
        {
            using (var context = new PrincipalContext(ContextType.Machine))
            using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
            {
                foreach (var result in searcher.FindAll())
                {
                    if (result is UserPrincipal user)
                    {
                        users[user.SamAccountName] = user.Description;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Проверка не пройдена: {ex.Message}");
        }

        return users;
    }


    private async Task<ExcelDataModel> FillUserOrGroupDescriptionsAsync(ExcelDataModel dataItem)
    {
        foreach (var ac in dataItem.AccessUsers)
        {
            var userName = ac[1];
            if (SidExtensions.TryParseSid(userName, out var _))
            {
                if (!SidExtensions.TryLookupAccountSid(dataItem.ServerName, userName, out string accountName,
                        out string description)) continue;
                ac[1] = accountName;
                ac[0] = description;
            }
            else
            {
                var identity = userName.Split('\\')[1];
                if (LocalGroups.ContainsKey(identity))
                {
                    ac[0] = LocalGroups[identity];
                }

                if (LocalUsers.ContainsKey(identity))
                {
                    ac[0] = LocalUsers[identity];
                }

                if (_isDomainAvailable)
                {
                    ac[0] = await GetDescFromActiveDirectoryAsync(identity);
                }
            }
        }

        return dataItem;
    }

    private async Task<string> GetDescFromActiveDirectoryAsync(string userName)
    {
        DirectorySearcher searcher;
        try
        {
            var entry = new DirectoryEntry(_settingsService.GetString("AppSettings:DefaultLDAPPath"));
            searcher = new DirectorySearcher(entry);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        searcher.Filter = $"(sAMAccountName={userName})";
        searcher.PropertiesToLoad.Add("description");
        var result = searcher.FindOne();
        if (result != null)
        {
            return result.Properties["description"][0].ToString();
        }

        searcher.Filter = $"(cn={userName})";
        searcher.PropertiesToLoad.Add("description");
        result = searcher.FindOne();
        if (result != null)
        {
            return result.Properties["description"][0].ToString();
        }

        return "Нет описания";
    }
}