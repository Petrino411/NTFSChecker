using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NTFSChecker.Avalonia.Extentions;

namespace NTFSChecker.Avalonia;

public partial class SettingsForm : Window
{
    private readonly ILogger<SettingsForm> _logger;
    private readonly IConfigurationRoot  _config;

    public SettingsForm(ILogger<SettingsForm> logger, IConfigurationRoot  config)
    {
        InitializeComponent();
        _logger = logger;
        _config = config;
        LoadSettings();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadSettings()
    {
        var colorMainPicker = this.FindControl<ColorPicker>("ColorMainPicker");
        var colorCurPicker = this.FindControl<ColorPicker>("ColorCurPicker");
        var colorRightPicker = this.FindControl<ColorPicker>("ColorRightPicker");
        var allExportCheckBox = this.FindControl<CheckBox>("AllExportCheckBox");
        var ignoreUndefinedCheckBox = this.FindControl<CheckBox>("IgnoreUndefinedCheckBox");
        var domainTextBox = this.FindControl<TextBox>("DomainTextBox");

        // Загружаем цвета из конфига (HEX -> Avalonia.Media.Color)
        colorMainPicker.Color = ColorExtentions.FromHex(_config["AppSettings:MainDirColor"]);
        colorCurPicker.Color = ColorExtentions.FromHex(_config["AppSettings:CurDirColor"]);
        colorRightPicker.Color = ColorExtentions.FromHex(_config["AppSettings:RightsColor"]);

        allExportCheckBox.IsChecked = bool.Parse(_config["AppSettings:ExportAll"]);
        ignoreUndefinedCheckBox.IsChecked = bool.Parse(_config["AppSettings:IgnoreUndefined"]);
        domainTextBox.Text = _config["AppSettings:DefaultLDAPPath"];
    }

    private void BtnOK_Click(object sender, RoutedEventArgs e)
    {
        var colorMainPicker = this.FindControl<ColorPicker>("ColorMainPicker");
        var colorCurPicker = this.FindControl<ColorPicker>("ColorCurPicker");
        var colorRightPicker = this.FindControl<ColorPicker>("ColorRightPicker");
        var domainTextBox = this.FindControl<TextBox>("DomainTextBox");

        // Сохраняем цвета в конфиг (Avalonia.Media.Color -> HEX)
        _config["AppSettings:MainDirColor"] = ColorExtentions.ToHex(colorMainPicker.Color);
        _config["AppSettings:CurDirColor"] = ColorExtentions.ToHex(colorCurPicker.Color);
        _config["AppSettings:RightsColor"] = ColorExtentions.ToHex(colorRightPicker.Color);
        _config["AppSettings:DefaultLDAPPath"] = domainTextBox.Text;

        Close();
    }

    private void IgnoreUndefinedCheckedChanged(object sender, RoutedEventArgs e)
    {
        var ignoreUndefinedCheckBox = this.FindControl<CheckBox>("IgnoreUndefinedCheckBox");
        _config["AppSettings:IgnoreUndefined"] = ignoreUndefinedCheckBox.IsChecked.ToString();
    }

    private void ExportAllCheckedChanged(object sender, RoutedEventArgs e)
    {
        var allExportCheckBox = this.FindControl<CheckBox>("AllExportCheckBox");
        _config["AppSettings:ExportAll"] = allExportCheckBox.IsChecked.ToString();
    }
}

