using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace NTFSChecker.Models;

public class ExcelDataModel
{
    public string ServerName { get; set; }
    public string DirName { get; set; }
    public string Ip { get; set; }
    public string Purpose { get; set; }
    public Tuple<AuthorizationRuleCollection, List<string>> AccessUsers { get; set; }

    public ExcelDataModel()
    {
        
    }
    
    public ExcelDataModel(string dirName, List<string> descriptionUsers, AuthorizationRuleCollection accessUsers)
    {
        DirName = dirName;
        AccessUsers[accessUsers] = descriptionUsers;
    }



}