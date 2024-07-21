using System.Windows.Controls;

namespace WpfTemplateStudio.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
