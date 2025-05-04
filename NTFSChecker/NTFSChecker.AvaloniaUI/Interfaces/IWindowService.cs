using Avalonia.Controls;

namespace NTFSChecker.AvaloniaUI.Interfaces;

public interface  IWindowService
{
    void ShowWindow<TWindow, TViewModel>()
        where TWindow : Window
        where TViewModel : class;
}