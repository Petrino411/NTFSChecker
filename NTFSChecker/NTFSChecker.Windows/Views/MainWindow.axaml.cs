using Avalonia.Controls;
using NTFSChecker.Windows.ViewModels;

namespace NTFSChecker.Windows.Views;

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