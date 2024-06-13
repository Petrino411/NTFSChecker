using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NTFSChecker.DTO;
using NTFSChecker.Services;

namespace NTFSChecker
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly ExcelWriter _excelWriter;
        private string SelectedFolderPath { get; set; }

        private readonly DirectoryChecker _directoryChecker;

        public MainForm(ILogger<MainForm> logger, ExcelWriter excelWriter, DirectoryChecker directoryChecker)
        {
            _directoryChecker = directoryChecker;
            _excelWriter = excelWriter;
            _logger = logger;
            InitializeComponent();

            _directoryChecker.ProgressUpdated += OnProgressUpdated;
            _directoryChecker.LogMessage += OnLogMessage;
            _directoryChecker.TotalItemsUpdated += OnTotalItemsUpdated;
            
            bool.TryParse(ConfigurationManager.AppSettings["IgnoreUndefined"], out bool flag); 
            IgnoreUndefinedTool.Checked = flag;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            ListLogs.Items.Clear();
            progressBar.Value = 0;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Выберите элемент";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolderPath = Path.GetDirectoryName(openFileDialog.FileName);
                    txtFolderPath.Text = SelectedFolderPath;
                    LogToUI($"Выбрана папка: {SelectedFolderPath}");
                    
                }
            }
        }

        private async void BtnCheck_Click(object sender, EventArgs e)
        {
            _directoryChecker.RootData.Clear();
            progressBar.Value  =  0;
            ListLogs.Items.Clear();
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            try
            {
                await Task.Run(async () => { await _directoryChecker.CheckDirectoryAsync(txtFolderPath.Text); });
                LogToUI("Операция прошла успешно");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }
        }
        
        private async void ExportToExcelClick(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                try
                {
                    Invoke(new Action(() => Cursor  = Cursors.WaitCursor));
                    
                    if (_directoryChecker.RootData.Count == 0)
                    {
                        Invoke(new Action(() => Cursor  = Cursors.Default));
                        return;
                    }

                    List<ExcelDataModel> data = new List<ExcelDataModel>();
                    var headers = new List<string>
                    {
                        "Наименование сервера",
                        "IP",
                        "Каталог",
                        "Назначение",
                        "Кому предоставлен доступ",
                        "Назначение прав доступа",
                        "Права доступа",
                        "Тип прав"
                    };

                    if (!AllExportTool.Checked)
                    {
                        data.Add(_directoryChecker.RootData.FirstOrDefault());
                        data.AddRange(_directoryChecker.RootData.Where(x => x.ChangesFlag).ToList());
                    }
                    else
                    {
                        data = _directoryChecker.RootData;
                    }
                    
                    _excelWriter.CreateNewFile();
                    await _excelWriter.SetTableHeadAsync(headers);
                    await _excelWriter.WriteDataAsync(data);
                    await _excelWriter.CreateLegendAsync();
                    await _excelWriter.AutoFitColumnsAndRowsAsync();

                    await _excelWriter.SaveTempAndShowAsync();
                    Invoke(new Action(() => Cursor  = Cursors.Default));
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    _logger.LogError(ex.ToString());
                }
            });
        }
        
        
        private void OnTotalItemsUpdated(object sender, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => progressBar.Maximum = progress));
            }
            else
            {
                progressBar.Maximum = progress;
            }
        }

        private void OnProgressUpdated(object sender, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => progressBar.Value += progress));
            }
            else
            {
                progressBar.Value += progress;
            }
        }

        private void OnLogMessage(object sender, string message)
        {
            LogToUI(message);
        }

        private void LogToUI(string message)
        {
            
            if (InvokeRequired)
            {
                Invoke(new Action(() => ListLogs.Items.Add(message)));
                Invoke(new Action(() => ListLogs.SelectedItem = message));
            }
            else
            {
                ListLogs.Items.Add(message);
                ListLogs.SelectedItem = message;
            }
            _logger.LogInformation(message);
        }

        

        private void цветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myForm = Program.ServiceProvider.GetRequiredService<ColorSettingsForm>();
            myForm.Show();
        }

        private void IgnoreUndefinedTool_CheckedChanged(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["IgnoreUndefined"].Value = IgnoreUndefinedTool.Checked.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
