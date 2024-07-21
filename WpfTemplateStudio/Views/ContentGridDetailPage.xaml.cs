using System.Windows.Controls;

using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Views;

public partial class ContentGridDetailPage : Page
{
    public ContentGridDetailPage(ContentGridDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
