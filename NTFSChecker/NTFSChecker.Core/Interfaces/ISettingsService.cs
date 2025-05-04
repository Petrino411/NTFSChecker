using NTFSChecker.Core.Models;

namespace NTFSChecker.Core.Interfaces;

public interface ISettingsService
{
    public AppSettings Load();
    public string GetString(string key);
    public T GetSection<T>(string sectionName) where T : new();
    public AppSettings GetAppSettings();
    public void Save(AppSettings settings);

}