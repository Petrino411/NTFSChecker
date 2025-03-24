using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NTFSChecker.Avalonia.Extentions;

namespace NTFSChecker.Avalonia;

public partial class SettingsForm : Window
{
    private readonly ILogger<SettingsForm> _logger;
    private readonly IConfigurationRoot _config;

    public SettingsForm(ILogger<SettingsForm> logger, IConfigurationRoot config)
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
        var hexMainTextBox = this.FindControl<TextBox>("HexMainTextBox");
        var hexCurTextBox = this.FindControl<TextBox>("HexCurTextBox");
        var hexRightTextBox = this.FindControl<TextBox>("HexRightTextBox");

        // Загружаем цвета из конфига
        hexMainTextBox.Text = _config["AppSettings:MainDirColor"];
        hexCurTextBox.Text = _config["AppSettings:CurDirColor"];
        hexRightTextBox.Text = _config["AppSettings:RightsColor"];

        // Инициализируем предпросмотр
        UpdatePreview(hexMainTextBox, "PreviewMainBorder");
        UpdatePreview(hexCurTextBox, "PreviewCurBorder");
        UpdatePreview(hexRightTextBox, "PreviewRightBorder");
    }

    private void UpdatePreview(TextBox textBox, string borderName)
    {
        var border = this.FindControl<Border>(borderName);
        try
        {
            var color = ColorExtentions.FromHex(textBox.Text);
            border.Background = new SolidColorBrush(color);
        }
        catch
        {
            border.Background = new SolidColorBrush(Colors.Transparent);
        }
    }

    private void HexTextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = (TextBox)sender;
        var borderName = textBox.Name.Replace("Hex", "Preview").Replace("TextBox", "Border");
        UpdatePreview(textBox, borderName);
    }


    private void BtnOK_Click(object sender, RoutedEventArgs e)
    {
        var hexMainTextBox = this.FindControl<TextBox>("HexMainTextBox");
        var hexCurTextBox = this.FindControl<TextBox>("HexCurTextBox");
        var hexRightTextBox = this.FindControl<TextBox>("HexRightTextBox");
        var domainTextBox = this.FindControl<TextBox>("DomainTextBox");

        // Сохраняем цвета в конфиг
        _config["AppSettings:MainDirColor"] = hexMainTextBox.Text;
        _config["AppSettings:CurDirColor"] = hexCurTextBox.Text;
        _config["AppSettings:RightsColor"] = hexRightTextBox.Text;
        _config["AppSettings:DefaultLDAPPath"] = domainTextBox.Text;
        ConfigSave();

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

    private void ConfigSave()
    {
        var config = App.Configuration.AsEnumerable().ToDictionary();
        var json = JsonSerializer.Serialize(config);
        File.WriteAllText("appsettings.json", json);
    }
}