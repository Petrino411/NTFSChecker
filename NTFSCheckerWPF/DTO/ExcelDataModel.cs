using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using NTFSChecker.Services;

namespace NTFSChecker.DTO;

public class ExcelDataModel
{
    public string ServerName { get; set; }
    public string DirName { get; set; }
    public string Ip { get; set; }
    public string Purpose { get; set; }
    public List<List<string>> AccessUsers { get; private set; } = [];
    public bool ChangesFlag { get; set; }


    public ExcelDataModel()
    {
    }

    private async void SetAccessUsers(AuthorizationRuleCollection rules, UserGroupHelper userGroupHelper)
    {
        AccessUsers.Clear();
        foreach (FileSystemAccessRule rule in rules)
        {
            string accessType;
            try
            {
                accessType = rule.FileSystemRights.ToString();
                if (int.TryParse(accessType, out _))
                {
                    accessType = $"{rule.FileSystemRights.ToString()}: Права не определены";
                }
            }
            catch
            {
                accessType = "Права не определены";
            }

            var item = new List<string>()
            {
                userGroupHelper.GetGroupDescriptionAsync(rule),
                rule.IdentityReference.Value, accessType, rule.AccessControlType.ToString()
            };
            AccessUsers.Add(item);

            AccessUsers.Sort((x, y) => string.Compare(x[1], y[1], StringComparison.OrdinalIgnoreCase));
        }
    }

    public ExcelDataModel(string path, UserGroupHelper userGroupHelper, AuthorizationRuleCollection rules,
        bool areChanges)
    {
        DirName = path;
        SetAccessUsers(rules, userGroupHelper);
        ChangesFlag = areChanges;
    }
}