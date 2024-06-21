using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.DirectoryServices;

namespace NTFSChecker.Services;

public class UserGroupHelper
{
    private Dictionary<string, string> LocalGroups { get; set; }
    private Dictionary<string, string> LocalUsers { get; set; }

    private bool _isDomainAvailable;


    private ILogger<UserGroupHelper> _logger;

    public UserGroupHelper(ILogger<UserGroupHelper> logger)
    {
        _logger = logger;
        CheckDomainAvailability();
        LocalGroups = GetLocalUserGroupsAsync().Result;
        LocalUsers = GetLocalUsersAsync().Result;
    }
    
    public void CheckDomainAvailability()
    {
        var path = ConfigurationManager.AppSettings["DefaultLDAPPath"];
        try
        {
            using (var rootDSE = new DirectoryEntry(path))
            {
                _isDomainAvailable = rootDSE.Properties["defaultNamingContext"].Value != null;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Domain {path} is not available.");
            _isDomainAvailable = false;
        }
    }

    public async Task<string> GetDescriptionAsync(string identity)
    {
        try
        {
            var groupDescription = await GetUserOrGroupDescriptionsAsync(identity.Split('\\')[1]);
            if (groupDescription != null)
            {
                return groupDescription;
            }
            
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка в получении описания: {ex.Message}? {identity}");
        }

        return "Нет описания";
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


    private async Task<string> GetUserOrGroupDescriptionsAsync(string userName)
    {
        if (LocalGroups.ContainsKey(userName))
        {
            return LocalGroups[userName];
        }

        if (LocalUsers.ContainsKey(userName))
        {
            return LocalUsers[userName];
        }

        if (_isDomainAvailable)
        {
            return await GetDescFromActiveDirectoryAsync(userName);
        }

        return "Нет описания";


    }

    private async Task<string> GetDescFromActiveDirectoryAsync(string userName)
    {
        DirectorySearcher searcher;
        try
        {
            var entry = new DirectoryEntry(System.Configuration.ConfigurationManager.AppSettings["DefaultLDAPPath"]);
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