using System.Windows.Controls;

using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
