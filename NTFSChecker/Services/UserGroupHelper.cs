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
    private Dictionary<string, string> Groups { get; set; }
    private ILogger<UserGroupHelper> _logger;

    public UserGroupHelper(ILogger<UserGroupHelper> logger)
    {
        _logger = logger;
        Groups = GetUserGroupsAsync().Result;

    }
    public async Task<string> GetGroupDescriptionAsync(FileSystemAccessRule rule)
    {
        try
        {
            var identity = rule.IdentityReference as NTAccount;

            var groupDescription =await GetUserGroupDescriptionsAsync(identity.Value.Split('\\')[1]);
            return  groupDescription;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка в получении прав: {ex.Message}");
        }
        return  null;
        
    }
    
    
    private async Task<Dictionary<string, string>>  GetUserGroupsAsync()
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

    private  async Task<string>  GetUserGroupDescriptionsAsync(string userName)
    {
        if (Groups.ContainsKey(userName))
        {
            return Groups[userName];
        }
        return "Нет групп или описания";
    }
}