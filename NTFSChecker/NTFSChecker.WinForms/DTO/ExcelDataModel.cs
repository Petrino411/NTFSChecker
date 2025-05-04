using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace NTFSChecker.WinForms.DTO;

public class ExcelDataModel
{
    public string ServerName { get; }
    public string DirName { get; }
    public string Ip { get; }
    public string Purpose { get; }
    public List<List<string>> AccessUsers { get; } = [];
    public bool ChangesFlag { get; }
    
    public ExcelDataModel(string remoteName, string path, IEnumerable<FileSystemAccessRule> rules,
        bool areChanges)
    {
        if (!string.IsNullOrEmpty(remoteName))
        {
            ServerName = remoteName;
        }
        DirName = path;
        SetAccessUsers(rules);
        ChangesFlag = areChanges;
    }

    private void SetAccessUsers(IEnumerable<FileSystemAccessRule> rules)
    {
        AccessUsers.Clear();
        foreach (var rule in rules)
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
            var item  = new List<string>(){ "Нет описания", 
                rule.IdentityReference.Value,  accessType,  rule.AccessControlType.ToString()};
            AccessUsers.Add(item);
            
            AccessUsers.Sort((x, y) => string.Compare(x[1], y[1], StringComparison.OrdinalIgnoreCase));
            
        }        
    }


}