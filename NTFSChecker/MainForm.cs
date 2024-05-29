using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;


namespace NTFSChecker
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        
        private string SelectedFolderPath { get; set; }

        public MainForm(ILogger<MainForm> logger )
        {
            _logger = logger;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                
                openFileDialog.InitialDirectory = "C:\\Users\\Petrino\\Desktop\\PracticeFNS\\NTFSChecker";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Выберите";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolderPath = Path.GetDirectoryName(openFileDialog.FileName);
                    txtFolderPath.Text = SelectedFolderPath;
                    ListLogs.Items.Add($"Выбрана папка: {SelectedFolderPath}");
                    _logger.LogInformation($"Выбрана папка: {SelectedFolderPath}");
                    txtFolderPath.Text = SelectedFolderPath;
                }
            }
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (txtFolderPath.Text is null) return;
            CheckDirectory(txtFolderPath.Text);

        }

        private async static Task<bool> CompareAccessRules(AuthorizationRuleCollection acl1, AuthorizationRuleCollection acl2)
        {
            if (acl1.Count != acl2.Count)
            {
                return false;
            }

            foreach (AuthorizationRule rule in acl1)
            {
                var match = false;
                foreach (AuthorizationRule rule2 in acl2)
                {
                    if (rule is not FileSystemAccessRule fsRule1 || rule2 is not FileSystemAccessRule fsRule2) continue;
                    if (fsRule1.IdentityReference.Value != fsRule2.IdentityReference.Value ||
                        fsRule1.AccessControlType != fsRule2.AccessControlType ||
                        fsRule1.FileSystemRights != fsRule2.FileSystemRights) continue;
                    match = true;
                    break;
                }

                if (!match)
                {
                    return false;
                }
            }

            return true;
        }
        private async void CheckDirectory(string path)
        {
            var rootAcl = GetAccessRules(path);
            await CheckDirectory(path, rootAcl);
        }

        private async Task CheckDirectory(string path, AuthorizationRuleCollection rootAcl)
        {
            try
            {
                // Проверяем текущую папку
                var currentAcl = GetAccessRules(path);
                if (!await CompareAccessRules(rootAcl, currentAcl))
                {
                    _logger.LogWarning($"Различия в правах доступа обнаружены в папке: {path}");
                    
                    ListLogs.Items.Add($"Различия в правах доступа обнаружены в папке: {path}");
                }

                progressBar1.Value++;
                foreach (var directory in Directory.GetDirectories(path))
                {
                    CheckDirectory(directory, rootAcl);
                }

                var files = Directory.GetFiles(path);
                if (Directory.EnumerateFiles(path).ToList().Count != 0)
                {
                    foreach (var file in files)
                    {
                        var fileAcl = GetFileAccessRules(file);
                        if (!await CompareAccessRules(rootAcl, fileAcl))
                        {
                            ListLogs.Items.Add($"Различия в правах доступа обнаружены в файле: {file}");
                            _logger.LogWarning($"Различия в правах доступа обнаружены в файле: {file}");
                        }
                    }
                }

                
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, $"Недостаточно прав для доступа к: {path}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обработке папки {path}");
            }
        }

        static AuthorizationRuleCollection GetAccessRules(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directorySecurity = directoryInfo.GetAccessControl();
            return directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
        }
        static AuthorizationRuleCollection GetFileAccessRules(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            return fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
        }
    }
}