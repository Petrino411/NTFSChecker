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

        public event EventHandler<int> ProgressUpdated;
        public event EventHandler<int> TotalItemsUpdated;
        public event EventHandler<string> LogMessage;

        public DirectoryChecker(ILogger<DirectoryChecker> logger, UserGroupHelper userGroupHelper)
        {
            _logger = logger;
            _userGroupHelper = userGroupHelper;
        }

        public async Task CheckDirectoryAsync(string path)
        {
            var rootAcl = await GetAccessRules(path);
            var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                             Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;

            TotalItemsUpdated?.Invoke(this, totalItems + 1);
            
            async Task CheckDirectoryAsyncInternal(string subPath)
            {
                var currentAcl = await GetAccessRules(subPath);
                
                

                if (!await CompareAccessRules(rootAcl, currentAcl))
                {
                    RootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, true));
                    LogMessage?.Invoke(this, $"Различия в правах доступа обнаружены в папке: {subPath}");
                }
                else
                {
                    RootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, false));
                }
                ProgressUpdated?.Invoke(this, 1);

                foreach (var directory in Directory.GetDirectories(subPath))
                {
                    await CheckDirectoryAsyncInternal(directory);
                }

                foreach (var file in Directory.GetFiles(subPath))
                {
                    var fileAcl = await GetFileAccessRules(file);
                    if (!await CompareAccessRules(rootAcl, fileAcl))
                    {
                        RootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, true));
                        LogMessage?.Invoke(this, $"Различия в правах доступа обнаружены в файле: {file}");
                    }
                    else
                    {
                        RootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, false));
                    }
                    ProgressUpdated?.Invoke(this, 1);
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
                var directorySecurity = directoryInfo.GetAccessControl();
                var acl = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                return FilterAccessRules(acl).ToList();
                

                
            });
        }

        private async Task<IEnumerable<FileSystemAccessRule>> GetFileAccessRules(string filePath)
        {
            return await Task.Run(() =>
            {
                var fileInfo = new FileInfo(filePath);
                var fileSecurity = fileInfo.GetAccessControl();
                var acl = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
                return FilterAccessRules(acl).ToList();
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
