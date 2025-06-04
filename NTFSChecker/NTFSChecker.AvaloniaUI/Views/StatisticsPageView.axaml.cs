using Avalonia.Controls;
using Avalonia.Input;
using NTFSChecker.AvaloniaUI.ViewModels;
using NTFSChecker.Windows.Extensions;

namespace NTFSChecker.AvaloniaUI.Views;

public partial class StatisticsPageView : UserControl
{
    public StatisticsPageView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            WindowsShellInterop.ShowFileProperties(textBlock.Text);
        }

        
    }
}