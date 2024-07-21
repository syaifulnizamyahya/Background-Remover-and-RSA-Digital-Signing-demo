using System.Windows.Controls;

using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Views;

public partial class ListDetailsPage : Page
{
    public ListDetailsPage(ListDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
