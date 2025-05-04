using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NTFSChecker.Core.Interfaces;
using NTFSChecker.Core.Models;
namespace NTFSChecker.Core.Services;

public class SettingsService : ISettingsService
{
    private readonly IConfigurationRoot _configuration;
    private const string ConfigFileName = "appsettings.json";

    public SettingsService(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    public AppSettings Load()
    {
        var settings = new AppSettings();
        _configuration.Bind("AppSettings", settings);
        return settings;
    }
    
    public string GetString(string key)
    {
        return _configuration[key] ?? string.Empty;
    }

    public T GetSection<T>(string sectionName) where T : new()
    {
        var result = new T();
        _configuration.GetSection(sectionName).Bind(result);
        return result;
    }

    public AppSettings GetAppSettings()
    {
        return GetSection<AppSettings>("AppSettings");
    }

    public void Save(AppSettings settings)
    {
        var fullConfig = _configuration.AsEnumerable().ToDictionary(kvp => kvp.Key, kvp => kvp.Value ?? "");
        
        fullConfig["AppSettings:MainDirColor"] = settings.MainDirColor;
        fullConfig["AppSettings:CurDirColor"] = settings.CurDirColor;
        fullConfig["AppSettings:RightsColor"] = settings.RightsColor;
        fullConfig["AppSettings:DefaultLDAPPath"] = settings.DefaultLDAPPath;
        fullConfig["AppSettings:IgnoreUndefined"] = settings.IgnoreUndefined.ToString();
        fullConfig["AppSettings:ExportAll"] = settings.ExportAll.ToString();
        
        var json = JsonSerializer.Serialize(new { AppSettings = settings }, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigFileName, json);
    }
    
}