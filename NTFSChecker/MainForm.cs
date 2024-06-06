using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using NTFSChecker.Models;
using NTFSChecker.Services;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;


namespace NTFSChecker
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;

        private readonly ExcelWriter _excelWriter;
        private string SelectedFolderPath { get; set; }
        private UserGroupHelper _userGroupHelper;
        private List<ExcelDataModel> rootData { get; set; } = [];
        private bool changesCheckBox { get; set; } = false;


        public MainForm(ILogger<MainForm> logger, ExcelWriter excelWriter, UserGroupHelper userGroupHelper)
        {
            _userGroupHelper = userGroupHelper;
            _excelWriter = excelWriter;
            _logger = logger;
            InitializeComponent();
        }


        private void BtnOpen_Click(object sender, EventArgs e)
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
                    LogToUI($"Выбрана папка: {SelectedFolderPath}");
                    _logger.LogInformation($"Выбрана папка: {SelectedFolderPath}");
                    txtFolderPath.Text = SelectedFolderPath;
                }
            }
        }

        private async void BtnCheck_Click(object sender, EventArgs e)
        {
            rootData.Clear();
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            try
            {
                await CheckDirectoryAsync(txtFolderPath.Text);
                LogToUI("Операция прошла успешно");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }
        }

        private async static Task<bool> CompareAccessRules(AuthorizationRuleCollection acl1,
            AuthorizationRuleCollection acl2)
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

        private async Task CheckDirectoryAsync(string path)
        {
            var rootAcl = GetAccessRules(path);
            var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                             Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
            progressBar.Maximum = totalItems + 1;
            progressBar.Value = 0;
            await CheckDirectoryAsync(path, rootAcl);
        }

        private async Task CheckDirectoryAsync(string path, AuthorizationRuleCollection rootAcl)
        {
            _logger.LogInformation($"Проверяется: {path}");
            LogToUI($"Проверяется: {path}");
            try
            {
                var currentAcl = GetAccessRules(path);

                if (!await CompareAccessRules(rootAcl, currentAcl))
                {
                    rootData.Add(new ExcelDataModel(path, currentAcl,
                        await _userGroupHelper.GetAccessRulesWithGroupDescriptionAsync(path), true));


                    _logger.LogWarning($"Различия в правах доступа обнаружены в папке: {path}");

                    LogToUI($"Различия в правах доступа обнаружены в папке: {path}");
                }
                else
                {
                    rootData.Add(new ExcelDataModel(path, currentAcl,
                        await _userGroupHelper.GetAccessRulesWithGroupDescriptionAsync(path), false));
                }

                UpdateProgress();

                foreach (var directory in Directory.GetDirectories(path))
                {
                    await CheckDirectoryAsync(directory, rootAcl);
                }


                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var fileAcl = GetFileAccessRules(file);

                    if (!await CompareAccessRules(rootAcl, fileAcl))
                    {
                        rootData.Add(new ExcelDataModel(file, fileAcl,
                            await _userGroupHelper.GetAccessRulesWithGroupDescriptionAsync(path), true));

                        LogToUI($"Различия в правах доступа обнаружены в файле: {file}");
                        _logger.LogWarning($"Различия в правах доступа обнаружены в файле: {file}");
                    }
                    else
                    {
                        rootData.Add(new ExcelDataModel(file, fileAcl,
                            await _userGroupHelper.GetAccessRulesWithGroupDescriptionAsync(path), false));
                    }

                    UpdateProgress();
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

        private AuthorizationRuleCollection GetAccessRules(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directorySecurity = directoryInfo.GetAccessControl();
            return directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
        }

        private AuthorizationRuleCollection GetFileAccessRules(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var fileSecurity = fileInfo.GetAccessControl();
            return fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
        }

        private void UpdateProgress()
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => progressBar.Value++));
            }
            else
            {
                progressBar.Value++;
            }
        }

        private void LogToUI(string message)
        {
            ListLogs.Items.Add(message);
            ListLogs.SelectedIndex = ListLogs.Items.Count - 1;
        }

        private async void ExportToExcelClick(object sender, EventArgs e)
        {
            try
            {
                List<ExcelDataModel> data = [];
                var headers = new List<string>
                {
                    "Наименование сервера",
                    "IP",
                    "Каталог",
                    "Назначение",
                    "Кому предоставлен доступ",
                    "Назначение прав доступа"
                };

                if (!changesCheckBox)
                {
                    data.Add(rootData[0]);
                    data.AddRange(rootData.Where(x => x.ChangesFlag).ToList());
                }
                else
                {
                    data = rootData;
                }
                
                _excelWriter.CreateNewFile();
                await _excelWriter.SetTableHeadAsync(headers);
                await _excelWriter.WriteDataAsync(data);
                _excelWriter.AutoFitColumnsAndRows();

                _excelWriter.SaveTempAndShow();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                _logger.LogError(ex.ToString());
            }
        }


        private void ChangesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            changesCheckBox = ChangesCheckBox.Checked;
        }
    }
}