using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        
        private readonly Stopwatch  _stopwatch;
        private string SelectedFolderPath { get; set; }

        private readonly DirectoryChecker _directoryChecker;
        private readonly UserGroupHelper _reGroupHelper;

        private int _files = 0;
        private int _directories = 0;
        
        public event EventHandler<int> ItemsCounted;

        public MainForm(ILogger<MainForm> logger, ExcelWriter excelWriter, DirectoryChecker directoryChecker, UserGroupHelper reGroupHelper)
        {
            InitializeComponent();
            _stopwatch  = new Stopwatch();
            ItemsCounted += OnItemsCounted;

            SelectedFolderPath = string.Empty;
            _directoryChecker = directoryChecker;
            _reGroupHelper = reGroupHelper;
            _excelWriter = excelWriter;
            _logger = logger;
            _directoryChecker.LogMessage += OnLogMessage;
            _directoryChecker.ProgressUpdate += OnProgressUpdate;
            
            bool.TryParse(ConfigurationManager.AppSettings["IgnoreUndefined"], out bool flag); 
            IgnoreUndefinedTool.Checked = flag;
            изменитьToolStripMenuItem.Text = ConfigurationManager.AppSettings["DefaultLDAPPath"];
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            StopWatchReset();
            Reset();

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
            StopWatchReset();
            StopWatchStart();
            Reset();
            DisableControls();
            
            if (string.IsNullOrEmpty(txtFolderPath.Text)) return;
            try
            {
                Task.Run(async () => { await CountItems(txtFolderPath.Text); });
                await Task.Run(async () => { await _directoryChecker.CheckDirectoryAsync(txtFolderPath.Text); });
                LogToUI("Операция прошла успешно");
                progressBar.Value = progressBar.Maximum;
                StopWatchStop();
                EnableControls();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }
        }

        private async Task CountItems(string path)
        {
            var totalItems = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length +
                             Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length;
            ItemsCounted?.Invoke(this, totalItems);
        }

        private void Reset()
        {
            progressBar.Value = 0;
            progressBar.Maximum = 100;
            _directoryChecker.RootData.Clear();
            ListLogs.Items.Clear();
            labelInfo.Text = $"Проверено:\nпапок:\nфайлов:";
            
        }

        private void OnItemsCounted(object sender, int count)
        {
            Invoke(new Action(() => progressBar.Maximum = count+1));
            Invoke(new Action(() => progressBar.Value = _files + _directories));
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

                    
                    var data  = await PrepareDataToExport();
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

        private async Task<List<ExcelDataModel>> PrepareDataToExport()
        {
            List<ExcelDataModel> data = new List<ExcelDataModel>();
            if (!AllExportTool.Checked)
            {
                data.Add(_directoryChecker.RootData.FirstOrDefault());
                data.AddRange(_directoryChecker.RootData.Where(x => x.ChangesFlag).ToList());
            }
            else
            {
                data = _directoryChecker.RootData;
            }
            
            foreach (var item in data)
            {
                foreach (var ac in item.AccessUsers)
                {
                    ac[0] = await _reGroupHelper.GetDescriptionAsync(ac[1]);
                }
            }
            return data;
            
        }

        private void OnProgressUpdate(object sender, (int dirs, int files) info )
        {
            _files = info.files;
            _directories  = info.dirs;
            var max= 0;
            Invoke(new Action(() =>  max = progressBar.Maximum));
            if (max > 100)
            {
                Invoke(new Action(() => progressBar.Value = _files + _directories));
            }


            Invoke(new Action(() => labelInfo.Text = $"Проверено:\nпапок:{info.dirs}\nфайлов:{info.files}"));
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

        private void StopWatchStart()
        {
            Timer1.Start();
            _stopwatch.Start();
        }
        private void StopWatchStop()
        {
            Timer1.Stop();
            _stopwatch.Stop();
        }
        
        private void StopWatchReset()
        {
            _stopwatch.Reset();
            labelTimer.Text = "00:00:00:000";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var elapsed = _stopwatch.Elapsed;
            labelTimer.Text =
                $"{Math.Floor(elapsed.TotalHours):00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}:{elapsed.Milliseconds:00}";
        }

        private void DisableControls()
        {
            BtnOpen.Enabled  =  false;
            BtnCheck.Enabled  =  false;
            txtFolderPath.Enabled =  false;
            menuStrip1.Enabled  =  false;
            
        }
        
        private void EnableControls()
        {
            BtnOpen.Enabled  =  true;
            BtnCheck.Enabled  =  true;
            txtFolderPath.Enabled =  true;
            menuStrip1.Enabled  =  true;
            
        }

    }
}
