namespace NTFSChecker.Core.Models;

public class ExcelDataModel
{
    public string ServerName { get; }
    public string DirName { get; }
    public string Ip { get; }
    public string Purpose { get; }
    public List<List<string>> AccessUsers { get; } = [];
    public bool ChangesFlag { get; }

    public ExcelDataModel(string remoteName, string path, List<List<string>> accessUsers,
        bool areChanges)
    {
        if (!string.IsNullOrEmpty(remoteName))
        {
            ServerName = remoteName;
        }

        DirName = path;
        AccessUsers = accessUsers;
        ChangesFlag = areChanges;
    }
}