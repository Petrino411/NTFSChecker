using Avalonia.Controls;
using Avalonia.Input;
using NTFSChecker.AvaloniaUI.ViewModels;


namespace NTFSChecker.AvaloniaUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
  
}