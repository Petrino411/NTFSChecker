using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NTFSChecker.DTO;

namespace NTFSChecker.Services
{
    public class DirectoryChecker
    {
        private readonly ILogger<DirectoryChecker> _logger;
        private readonly UserGroupHelper _userGroupHelper;

        public List<ExcelDataModel> RootData { get; set; } = new List<ExcelDataModel>();
        
        public event EventHandler<string> LogMessage;
        public event EventHandler<(int, int)> ProgressUpdate;

        public DirectoryChecker(ILogger<DirectoryChecker> logger, UserGroupHelper userGroupHelper)
        {
            _logger = logger;
            _userGroupHelper = userGroupHelper;
        }

        public async Task CheckDirectoryAsync(string path)
        {
            var rootAcl = await GetAccessRules(path);
            // var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
            //                  Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
            
            var dirs = 0;
            var files = 0;
            async Task CheckDirectoryAsyncInternal(string subPath)
            {
                var currentAcl = await GetAccessRules(subPath);
                if (currentAcl is not null)
                {
                    if (!await CompareAccessRules(rootAcl, currentAcl))
                    {
                        RootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, true));
                        LogMessage?.Invoke(this, $"Различия в правах доступа обнаружены в папке: {subPath}");
                    }
                    else
                    {
                        RootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, false));
                    }
                    dirs ++;
                    ProgressUpdate?.Invoke(this, (dirs,  files));
                    
                }
                else
                {
                    LogMessage?.Invoke(this, $"Ошибка при получении прав: {subPath}");
                }

                try
                {
                    foreach (var directory in Directory.GetDirectories(subPath))
                    {
                        await CheckDirectoryAsyncInternal(directory);
                    }

                }
                catch (UnauthorizedAccessException ex)
                {
                    LogMessage?.Invoke(this, $"Отказано в доступе:  {subPath}\n {ex.Message}");
                }
                catch (Exception e)
                {
                    LogMessage?.Invoke(this, $"Ошибка при получении прав: {subPath}");
                }

                try
                {
                    foreach (var file in Directory.GetFiles(subPath))
                    {
                        var fileAcl = await GetFileAccessRules(file);
                        if (fileAcl is not null)
                        {
                        
                            if (!await CompareAccessRules(rootAcl, fileAcl))
                            {
                                RootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, true));
                                LogMessage?.Invoke(this, $"Различия в правах доступа обнаружены в файле: {file}");
                            }
                            else
                            {
                                RootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, false));
                            }
                            files ++;
                            ProgressUpdate?.Invoke(this, (dirs,  files));
                        
                        }
                        else
                        {
                            LogMessage?.Invoke(this, $"Ошибка при получении прав: {subPath}");
                        }

                    }

                }
                catch (UnauthorizedAccessException ex)
                {
                    LogMessage?.Invoke(this, $"Отказано в доступе:  {subPath}\n {ex.Message}");
                }
                catch (Exception e)
                {
                    LogMessage.Invoke(this, $"Ошибка при получении прав: {subPath}\n  {e.Message}");
                }
                
            }
            await Task.Run(() => CheckDirectoryAsyncInternal(path));
        }

        private static async Task<bool> CompareAccessRules(IEnumerable<FileSystemAccessRule> acl1, IEnumerable<FileSystemAccessRule> acl2)
        {
            return await Task.Run(() =>
            {
                var acl1List = acl1.ToList();
                var acl2List = acl2.ToList();

                if (acl1List.Count != acl2List.Count)
                {
                    return false;
                }

                foreach (var rule in acl1List)
                {
                    bool match = acl2List.Any(rule2 =>
                        rule.IdentityReference.Value == rule2.IdentityReference.Value &&
                        rule.AccessControlType == rule2.AccessControlType &&
                        rule.FileSystemRights == rule2.FileSystemRights);

                    if (!match)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        private async Task<IEnumerable<FileSystemAccessRule>> GetAccessRules(string path)
        {
            return await Task.Run(() =>
            {
                var directoryInfo = new DirectoryInfo(path);
                try
                {
                    var directorySecurity = directoryInfo.GetAccessControl();
                    var acl = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                    return FilterAccessRules(acl).ToList();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка при получении прав доступа к файлу: {path}", path);
                    return null;
                }
                
            });
        }

        private async Task<IEnumerable<FileSystemAccessRule>> GetFileAccessRules(string filePath)
        {
            return await Task.Run(() =>
            {
                var fileInfo = new FileInfo(filePath);
                try
                {
                    var fileSecurity = fileInfo.GetAccessControl();
                    var acl = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
                    return FilterAccessRules(acl).ToList();

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка при получении прав доступа к файлу: {filePath}", filePath);
                    return null;
                    
                }

            });
        }
        
        private IEnumerable<FileSystemAccessRule> FilterAccessRules(AuthorizationRuleCollection acl)
        {
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IgnoreUndefined"],
                out bool flag);
            
            foreach (FileSystemAccessRule rule in acl)
            {
                if (flag)
                {
                    if (int.TryParse(rule.FileSystemRights.ToString(), out _))
                    {
                        continue;
                    }
                    
                }

                yield return rule;
            }
        }
    }
}
