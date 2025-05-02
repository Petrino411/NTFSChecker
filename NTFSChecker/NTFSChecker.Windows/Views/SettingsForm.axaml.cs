using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NTFSChecker.Windows.ViewModels;

namespace NTFSChecker.Windows.Views;

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