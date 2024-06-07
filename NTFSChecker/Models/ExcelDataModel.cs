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

    public bool ChangesFlag { get; set; }

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
                    string accessType;
                    try
                    {
                        accessType = fsRule.FileSystemRights.ToString();
                        if (int.TryParse(accessType, out _))
                        {
                            accessType = "Права не определены";
                        }
                    }
                    catch
                    {
                        accessType = "Права не определены";
                    }
                    
                   

                    if (accessType.StartsWith("Права не определены"))
                    {
                        AccessUsers.Add($"{identity.Value}, ({fsRule.FileSystemRights}): {accessType}");
                    }
                    else
                    {
                        AccessUsers.Add($"{identity.Value}: {accessType}");
                    }
                    
                }
            }
        }
    }

    public ExcelDataModel(string path, AuthorizationRuleCollection rules, List<string> descriptionUsers,
        bool areChanges)
    {
        DirName = path;
        SetAccessUsers(rules);
        DescriptionUsers = descriptionUsers;
        ChangesFlag = areChanges;
    }
}