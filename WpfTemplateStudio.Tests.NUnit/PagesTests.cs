using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NUnit.Framework;

using WpfTemplateStudio.Contracts.Services;
using WpfTemplateStudio.Core.Contracts.Services;
using WpfTemplateStudio.Core.Services;
using WpfTemplateStudio.Models;
using WpfTemplateStudio.Services;
using WpfTemplateStudio.ViewModels;
using WpfTemplateStudio.Views;

namespace WpfTemplateStudio.Tests.NUnit;

public class PagesTests
{
    private IHost _host;

    [SetUp]
    public void Setup()
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => c.SetBasePath(appLocation))
            .ConfigureServices(ConfigureServices)
            .Build();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Core Services
        services.AddSingleton<IFileService, FileService>();

        // Services
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        services.AddTransient<WebViewViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<ListDetailsViewModel>();
        services.AddTransient<DataGridViewModel>();
        services.AddTransient<ContentGridViewModel>();
        services.AddTransient<BlankViewModel>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    // TODO: Add tests for functionality you add to WebViewViewModel.
    [Test]
    public void TestWebViewViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(WebViewViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetWebViewPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(WebViewViewModel).FullName);
            Assert.AreEqual(typeof(WebViewPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to SettingsViewModel.
    [Test]
    public void TestSettingsViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(SettingsViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetSettingsPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(SettingsViewModel).FullName);
            Assert.AreEqual(typeof(SettingsPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to MainViewModel.
    [Test]
    public void TestMainViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(MainViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetMainPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
            Assert.AreEqual(typeof(MainPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to ListDetailsViewModel.
    [Test]
    public void TestListDetailsViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(ListDetailsViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetListDetailsPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(ListDetailsViewModel).FullName);
            Assert.AreEqual(typeof(ListDetailsPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to DataGridViewModel.
    [Test]
    public void TestDataGridViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(DataGridViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetDataGridPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(DataGridViewModel).FullName);
            Assert.AreEqual(typeof(DataGridPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to ContentGridViewModel.
    [Test]
    public void TestContentGridViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(ContentGridViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetContentGridPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(ContentGridViewModel).FullName);
            Assert.AreEqual(typeof(ContentGridPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to BlankViewModel.
    [Test]
    public void TestBlankViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(BlankViewModel));
        Assert.IsNotNull(vm);
    }

    [Test]
    public void TestGetBlankPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(BlankViewModel).FullName);
            Assert.AreEqual(typeof(BlankPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }
}
