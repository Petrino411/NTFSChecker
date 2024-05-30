using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using System.IO;

namespace NTFSChecker.Services;



public class UserGroupHelper
{
    public static List<string> GetAccessRulesWithGroupDescription(string path)
    {
        var accessRules = new List<string>();

        var security = File.GetAccessControl(path);
        var rules = security.GetAccessRules(true, true, typeof(NTAccount));

        foreach (FileSystemAccessRule rule in rules)
        {
            var identity = rule.IdentityReference as NTAccount;
            if (identity != null)
            {
                var groupDescription = GetGroupDescription(identity);
                var accessRuleDescription = $"{identity.Value}: {rule.AccessControlType} ({groupDescription})";
                accessRules.Add(accessRuleDescription);
            }
        }

        return accessRules;
    }

    private static string GetGroupDescription(NTAccount account)
    {
        try
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var group = GroupPrincipal.FindByIdentity(context, account.Value))
            {
                if (group != null)
                {
                    return group.Description ?? "No Description";
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }

        return "Not a Group or No Description";
    }
}
