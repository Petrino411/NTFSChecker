using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using static System.Drawing.ColorTranslator;

namespace NTFSChecker;

public partial class SettingsForm : Form
{
    private readonly ILogger<MainForm> _logger;

    private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

    public SettingsForm(ILogger<MainForm> logger)
    {
        InitializeComponent();
        _logger = logger;
        LoadSettings();
    }

    private void LoadSettings()
    {
        ColorMain.BackColor = FromHtml(ConfigurationManager.AppSettings["MainDirColor"]);
        ColorCur.BackColor = FromHtml(ConfigurationManager.AppSettings["CurDirColor"]);
        ColorRight.BackColor = FromHtml(ConfigurationManager.AppSettings["RightsColor"]);
        bool.TryParse(ConfigurationManager.AppSettings["ExportAll"], out var allExp);
        AllExportCheckBox.Checked = allExp;
        bool.TryParse(ConfigurationManager.AppSettings["IgnoreUndefined"], out var ignoreUndef);
        IgnoreUndefinedCheckBox.Checked = ignoreUndef;

        DomainTextBox.Text = ConfigurationManager.AppSettings["DefaultLDAPPath"];
    }

    private void ColorMain_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorMain.BackColor = colorDialog.Color;
        }
    }

    private void ColorCur_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorCur.BackColor = colorDialog.Color;
        }
    }

    private void ColorRight_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorRight.BackColor = colorDialog.Color;
        }
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
        _config.AppSettings.Settings["MainDirColor"].Value = ToHtml(ColorMain.BackColor);
        _config.AppSettings.Settings["CurDirColor"].Value = ToHtml(ColorCur.BackColor);
        _config.AppSettings.Settings["RightsColor"].Value = ToHtml(ColorRight.BackColor);
        _config.AppSettings.Settings["DefaultLDAPPath"].Value = DomainTextBox.Text;

        _config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
        Close();
    }


    private void IgnoreUndefinedCheckedChanged(object sender, EventArgs e)
    {
        _config.AppSettings.Settings["IgnoreUndefined"].Value = IgnoreUndefinedCheckBox.Checked.ToString();
        _config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }

    private void ExportAllCheckedChanged(object sender, EventArgs e)
    {
        _config.AppSettings.Settings["ExportAll"].Value = AllExportCheckBox.Checked.ToString();
        _config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }
}