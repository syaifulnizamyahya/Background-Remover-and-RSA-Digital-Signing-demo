using System.Windows.Controls;

using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Views;

public partial class BackgroundRemoverPage : Page
{
    public BackgroundRemoverPage(BackgroundRemoverViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
