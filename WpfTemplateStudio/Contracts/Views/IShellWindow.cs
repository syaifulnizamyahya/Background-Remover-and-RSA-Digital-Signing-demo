using System.Windows.Controls;

namespace WpfTemplateStudio.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
