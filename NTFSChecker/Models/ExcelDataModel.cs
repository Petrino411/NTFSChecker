using System.Security.AccessControl;

namespace NTFSChecker.Models;

public class ExcelDataModel
{
    public string ServerName { get; set; }
    public string DirName { get; set; }
    public string Ip { get; set; }
    public string Purpose { get; set; }
    public string DescriptionUsers { get; set; }
    public string AccessUsers { get; set; }

    public ExcelDataModel()
    {
        
    }
    
    public ExcelDataModel(string dirName, string ip, string purpose, string descriptionUsers, string accessUsers)
    {
        DirName = dirName;
        Ip = ip;
        Purpose = purpose;
        DescriptionUsers = descriptionUsers;
        AccessUsers = accessUsers;
    }

    public void SetAccessUsers(AuthorizationRuleCollection ruleCollection)
    {
        foreach (AuthorizationRule rule in ruleCollection)
        {
            AccessUsers += "\n" + rule.IdentityReference;
        }
    }


}