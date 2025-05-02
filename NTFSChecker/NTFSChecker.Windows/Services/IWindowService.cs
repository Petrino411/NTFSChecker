using Avalonia.Controls;

namespace NTFSChecker.Windows.Services;

public interface  IWindowService
{
    void ShowWindow<TWindow, TViewModel>()
        where TWindow : Window
        where TViewModel : class;
}