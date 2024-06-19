using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NTFSChecker.Services;

public class UserGroupHelper
{
    private Dictionary<string, string> LocalGroups { get; set; }
    private Dictionary<string, string> DomainGroups { get; set; }
    private Dictionary<string, string> LocalUsers { get; set; }
    private Dictionary<string, string> DomainUsers { get; set; }
    private ILogger<UserGroupHelper> _logger;

    public UserGroupHelper(ILogger<UserGroupHelper> logger)
    {
        _logger = logger;
        LocalGroups = GetLocalUserGroupsAsync().Result;
        DomainGroups = GetDomainUserGroupsAsync().Result;
        
        LocalUsers = GetLocalUsersAsync().Result;
        DomainUsers = GetDomainUsersAsync().Result;
        
    }
    public async Task<string> GetDescriptionAsync(FileSystemAccessRule rule)
    {
        try
        {
            var identity = rule.IdentityReference as NTAccount;

            var groupDescription =await GetUserOrGroupDescriptionsAsync(identity.Value.Split('\\')[1]);
            return  groupDescription;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка в получении прав: {ex.Message}? {rule.IdentityReference}");
        }
        return  null;
        
    }
    
    
    private async Task<Dictionary<string, string>>  GetLocalUserGroupsAsync()
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
    
    private async Task<Dictionary<string, string>>  GetDomainUserGroupsAsync()
    {
        var userGroups = new Dictionary<string, string>();
        try
        {
            using (var context = new PrincipalContext(ContextType.Domain))
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
    
    
    private async Task<Dictionary<string, string>> GetDomainUsersAsync()
    {
        var users = new Dictionary<string, string>();

        try
        {
            using (var context = new PrincipalContext(ContextType.Domain))
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

    private  async Task<string>  GetUserOrGroupDescriptionsAsync(string userName)
    {
        if (LocalGroups.ContainsKey(userName))
        {
            return LocalGroups[userName];
        }
        if (LocalUsers.ContainsKey(userName))
        {
            return LocalUsers[userName];
        }
        if (DomainGroups.ContainsKey(userName))
        {
            return DomainGroups[userName];
        }
        if (DomainUsers.ContainsKey(userName))
        {
            return DomainUsers[userName];
        }
        
        return "Нет групп или описания";
    }
}