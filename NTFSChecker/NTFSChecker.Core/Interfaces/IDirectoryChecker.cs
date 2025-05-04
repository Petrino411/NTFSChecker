using NTFSChecker.Core.Models;

namespace NTFSChecker.Core.Interfaces;

public interface IDirectoryChecker
{
    public List<ExcelDataModel> RootData { get; set; }
    public Task CheckDirectoryAsync(
        string path,
        Func<string, Task> logAction,
        Func<int, int, Task> progressAction);

}