using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Security.Principal;
using NTFSChecker.Services;

namespace NTFSChecker.DTO;

public class ExcelDataModel
{
    public string ServerName { get; set; }
    public string DirName { get; set; }
    public string Ip { get; set; }
    public string Purpose { get; set; }
    public List<List<string>> AccessUsers { get; set; } = [];
    public bool ChangesFlag { get; set; }
    

    public ExcelDataModel()
    {
        
        
    }

    private void SetAccessUsers(IEnumerable<FileSystemAccessRule> rules)
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
            var item  = new List<string>(){ "Нет описания", 
                rule.IdentityReference.Value,  accessType,  rule.AccessControlType.ToString()};
            AccessUsers.Add(item);
            
            AccessUsers.Sort((x, y) => string.Compare(x[1], y[1], StringComparison.OrdinalIgnoreCase));
            
        }        
    }

    public ExcelDataModel(string path, IEnumerable<FileSystemAccessRule> rules,
        bool areChanges)
    {
        DirName = path;
        SetAccessUsers(rules);
        ChangesFlag = areChanges;
    }
}