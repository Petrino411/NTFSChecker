using Avalonia.Controls;
using NTFSChecker.AvaloniaUI.ViewModels;

namespace NTFSChecker.AvaloniaUI.Views;

public partial class SettingsForm : Window
{
    public SettingsForm()
    {
        InitializeComponent();
    }

    public SettingsForm(SettingsWinViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseAction = Close;
    }
}