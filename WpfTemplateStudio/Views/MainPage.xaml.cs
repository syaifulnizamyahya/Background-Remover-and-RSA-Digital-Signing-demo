using System.Windows.Controls;

using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
