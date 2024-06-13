using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NTFSChecker.Services;
using static System.Drawing.ColorTranslator;

namespace NTFSChecker;

public partial class ColorSettingsForm : Form {
    
    private readonly ILogger<MainForm> _logger;
    
    private readonly Configuration _config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    
    public ColorSettingsForm(ILogger<MainForm> logger)
    {
        _logger = logger;

        InitializeComponent();
        ColorMain.BackColor = FromHtml(System.Configuration.ConfigurationManager.AppSettings["MainDirColor"]);
        ColorCur.BackColor = FromHtml(System.Configuration.ConfigurationManager.AppSettings["CurDirColor"]);
        ColorRight.BackColor = FromHtml(System.Configuration.ConfigurationManager.AppSettings["RightsColor"]);
        
    }

    private void ColorMain_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorMain.BackColor= colorDialog.Color;
            
        }

    }

    private void ColorCur_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorCur.BackColor= colorDialog.Color;
           
        }
        
    }

    private void ColorRight_Click(object sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            ColorRight.BackColor= colorDialog.Color;
            
        }
        
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
        _config.AppSettings.Settings["MainDirColor"].Value = ToHtml(ColorMain.BackColor);
        _config.AppSettings.Settings["CurDirColor"].Value = ToHtml(ColorCur.BackColor);
        _config.AppSettings.Settings["RightsColor"].Value = ToHtml(ColorRight.BackColor);
    

        _config.Save(ConfigurationSaveMode.Modified);
        
        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        
        Close();
        
    }
    
}