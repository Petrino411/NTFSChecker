using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using System.IO;
using System.Threading.Tasks;

public class UserGroupHelper
{
    private Dictionary<string, List<string>> Groups { get; set; }

    public UserGroupHelper()
    {
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
                    var accessRuleDescription = $"{identity.Value}: {rule.AccessControlType} (Groups: {string.Join(", ", groupDescription)})\n";
                    accessRules.Add(accessRuleDescription);
                }
            }
        }
        catch (Exception ex)
        {
            accessRules.Add($"Error retrieving access rules: {ex.Message}");
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
                            userGroups[member.SamAccountName].Add(group.Description);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Local machine lookup failed: {ex.Message}");
        }

        return userGroups;
    }

    private static List<string> GetUserGroupDescriptionsAsync(string userName, Dictionary<string, List<string>> userGroups)
    {
        if (userGroups.ContainsKey(userName))
        {
            return userGroups[userName];
        }
        return new List<string> { "No Groups or No Description" };
    }
}
