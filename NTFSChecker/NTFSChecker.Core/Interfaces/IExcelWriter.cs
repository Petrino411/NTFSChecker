namespace NTFSChecker.Core.Interfaces;

public interface IExcelWriter
{
    public Task CreateNewFile(string rootDir, List<string> headers);
}