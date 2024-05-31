using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;
using NTFSChecker.Services;

namespace NTFSChecker.Models;

public class ExcelDataModel
{
    public string ServerName { get; set; }
    public string DirName { get; set; }
    public string Ip { get; set; }
    public string Purpose { get; set; }
    public List<string> DescriptionUsers { get; set; } = [];
    public List<string> AccessUsers { get; private set; } = [];

    public ExcelDataModel()
    {
        
    }
    
    public void SetAccessUsers(AuthorizationRuleCollection rules)
    {
        AccessUsers.Clear();
        foreach (AuthorizationRule rule in rules)
        {
            if (rule is FileSystemAccessRule fsRule)
            {
                var identity = fsRule.IdentityReference as NTAccount;
                if (identity != null)
                {
                    var accessType = fsRule.FileSystemRights.ToString();
                    AccessUsers.Add($"{identity.Value}: {accessType}");
                }
            }
        }
    }
    
    public ExcelDataModel(string dirName, List<string> descriptionUsers, AuthorizationRuleCollection accessUsers)
    {
        DirName = dirName;
        SetAccessUsers(accessUsers);
        DescriptionUsers = descriptionUsers;

    }

    public ExcelDataModel(string path, AuthorizationRuleCollection rules, UserGroupHelper userGroupHelper)
    {
        DirName = path;
        SetAccessUsers(rules);
        DescriptionUsers = userGroupHelper.GetAccessRulesWithGroupDescriptionAsync(path).Result;
        
    }



}