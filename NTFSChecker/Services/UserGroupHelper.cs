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
    private Dictionary<string, List<string>> Groups { get; set; }
    private ILogger<UserGroupHelper> _logger;

    public UserGroupHelper(ILogger<UserGroupHelper> logger)
    {
        _logger = logger;
        Groups = GetUserGroups();

    }
    public async Task<List<string>> GetAccessRulesWithGroupDescriptionAsync(string path)
    {
        var accessRules = new List<string>();

        try
        {
            var security = File.GetAccessControl(path);
            var rules = security.GetAccessRules(true, true, typeof(NTAccount));

            
            foreach (FileSystemAccessRule rule in rules)
            {
                var identity = rule.IdentityReference as NTAccount;
                if (identity != null)
                {
                    var groupDescription = GetUserGroupDescriptionsAsync(identity.Value.Split('\\')[1], Groups);
                    var accessRuleDescription = $"{identity.Value}: Группы: {string.Join(";\n ", groupDescription)}\n";
                    accessRules.Add(accessRuleDescription);
                }
            }
        }
        catch (Exception ex)
        {
            accessRules.Add($"Ошибка в получении прав: {ex.Message}");
            _logger.LogError($"Ошибка в получении прав: {ex.Message}");
        }

        return accessRules;
    }
    
    
    private Dictionary<string, List<string>> GetUserGroups()
    {
        var userGroups = new Dictionary<string, List<string>>();

        try
        {
            using (var context = new PrincipalContext(ContextType.Machine))
            using (var searcher = new PrincipalSearcher(new GroupPrincipal(context)))
            {
                foreach (var result in searcher.FindAll())
                {
                    if (result is GroupPrincipal group)
                    {
                        foreach (var member in group.GetMembers())
                        {
                            if (!userGroups.ContainsKey(member.SamAccountName))
                            {
                                userGroups[member.SamAccountName] = new List<string>();
                            }
                            userGroups[member.SamAccountName].Add($"{group.Name}: {group.Description}");
                        }
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

    private static List<string> GetUserGroupDescriptionsAsync(string userName, Dictionary<string, List<string>> userGroups)
    {
        if (userGroups.ContainsKey(userName))
        {
            return userGroups[userName];
        }
        return new List<string> { "Нет групп или описания" };
    }
}