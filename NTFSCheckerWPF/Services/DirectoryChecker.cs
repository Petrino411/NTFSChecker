using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NTFSChecker.DTO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Threading;

namespace NTFSChecker.Services
{
    public class DirectoryChecker
    {
        private readonly ILogger<DirectoryChecker> _logger;
        public MainWindow MainWindow { get; set; }
        public List<ExcelDataModel> rootData { get; set; } = new List<ExcelDataModel>();
        private UserGroupHelper _userGroupHelper { get; set; }

        public DirectoryChecker(ILogger<DirectoryChecker> logger, UserGroupHelper userGroupHelper)
        {
            _logger = logger;
            _userGroupHelper = userGroupHelper;
        }

        public async Task CheckDirectoryAsync(string path, IProgress<(string message, double progressValue)> progress)
        {
            var rootAcl = await GetAccessRules(path);
            var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                             Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;

            Application.Current.Dispatcher.Invoke(() =>
            {
                MainWindow.ProgressBar.Maximum = totalItems;
            });
            
            double progressCounter = 0;

            async Task CheckDirectoryAsyncInternal(string subPath)
            {
                var currentAcl = await GetAccessRules(subPath);

                if (!await CompareAccessRules(rootAcl, currentAcl))
                {
                    rootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, true));
                    progressCounter++;
                    progress.Report(($"Различия в правах доступа обнаружены в папке: {subPath}", progressCounter));
                }
                else
                {
                    rootData.Add(new ExcelDataModel(subPath, _userGroupHelper, currentAcl, false));
                }

                foreach (var directory in Directory.GetDirectories(subPath))
                {
                    await CheckDirectoryAsyncInternal(directory);
                }

                foreach (var file in Directory.GetFiles(subPath))
                {
                    var fileAcl = await GetFileAccessRules(file);
                    if (!await CompareAccessRules(rootAcl, fileAcl))
                    {
                        rootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, true));
                        progressCounter++;
                        progress.Report(($"Различия в правах доступа обнаружены в файле: {file}", progressCounter));
                    }
                    else
                    {
                        rootData.Add(new ExcelDataModel(file, _userGroupHelper, fileAcl, false));
                    }
                }
                
            }

            await Task.Run(() => CheckDirectoryAsyncInternal(path));
        }

        private static async Task<bool> CompareAccessRules(AuthorizationRuleCollection acl1, AuthorizationRuleCollection acl2)
        {
            return await Task.Run(() =>
            {
                if (acl1.Count != acl2.Count)
                {
                    return false;
                }

                foreach (AuthorizationRule rule in acl1)
                {
                    bool match = false;
                    foreach (AuthorizationRule rule2 in acl2)
                    {
                        if (rule is FileSystemAccessRule fsRule1 && rule2 is FileSystemAccessRule fsRule2 &&
                            fsRule1.IdentityReference.Value == fsRule2.IdentityReference.Value &&
                            fsRule1.AccessControlType == fsRule2.AccessControlType &&
                            fsRule1.FileSystemRights == fsRule2.FileSystemRights)
                        {
                            match = true;
                            break;
                        }
                    }

                    if (!match)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        private async Task<AuthorizationRuleCollection>  GetAccessRules(string path)
        {
            return await Task.Run(() =>
            {
                var directoryInfo = new DirectoryInfo(path);
                var directorySecurity = directoryInfo.GetAccessControl();
                return directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
            });
            
        }

        private async Task<AuthorizationRuleCollection> GetFileAccessRules(string filePath)
        {
            return await Task.Run(() =>
            {
                var fileInfo = new FileInfo(filePath);
                var fileSecurity = fileInfo.GetAccessControl();
                return fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
            });
            
        }
    }
}
