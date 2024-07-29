using Microsoft.Extensions.Options;

using Moq;

using NUnit.Framework;

using WpfTemplateStudio.Contracts.Services;
using WpfTemplateStudio.Models;
using WpfTemplateStudio.ViewModels;

namespace WpfTemplateStudio.Tests.NUnit;

public class SettingsViewModelTests
{
    public SettingsViewModelTests()
    {
    }

    [Test]
    public void TestSettingsViewModel_SetCurrentTheme()
    {
        var mockThemeSelectorService = new Mock<IThemeSelectorService>();
        mockThemeSelectorService.Setup(mock => mock.GetCurrentTheme()).Returns(AppTheme.Light);
        var mockAppConfig = new Mock<IOptions<AppConfig>>();
        var mockSystemService = new Mock<ISystemService>();
        var mockApplicationInfoService = new Mock<IApplicationInfoService>();

        var settingsVm = new SettingsViewModel(mockAppConfig.Object, mockThemeSelectorService.Object, mockSystemService.Object, mockApplicationInfoService.Object);
        settingsVm.OnNavigatedTo(null);

        Assert.That(settingsVm.Theme, Is.EqualTo(AppTheme.Light));
        //Assert.AreEqual(AppTheme.Light, settingsVm.Theme);
    }

    [Test]
    public void TestSettingsViewModel_SetCurrentVersion()
    {
        var mockThemeSelectorService = new Mock<IThemeSelectorService>();
        var mockAppConfig = new Mock<IOptions<AppConfig>>();
        var mockSystemService = new Mock<ISystemService>();
        var mockApplicationInfoService = new Mock<IApplicationInfoService>();
        var testVersion = new Version(1, 2, 3, 4);
        mockApplicationInfoService.Setup(mock => mock.GetVersion()).Returns(testVersion);

        var settingsVm = new SettingsViewModel(mockAppConfig.Object, mockThemeSelectorService.Object, mockSystemService.Object, mockApplicationInfoService.Object);
        settingsVm.OnNavigatedTo(null);

        Assert.That(settingsVm.VersionDescription, Is.EqualTo($"WpfTemplateStudio - {testVersion}"));
        //Assert.AreEqual($"WpfTemplateStudio - {testVersion}", settingsVm.VersionDescription);
    }

    [Test]
    public void TestSettingsViewModel_SetThemeCommand()
    {
        var mockThemeSelectorService = new Mock<IThemeSelectorService>();
        var mockAppConfig = new Mock<IOptions<AppConfig>>();
        var mockSystemService = new Mock<ISystemService>();
        var mockApplicationInfoService = new Mock<IApplicationInfoService>();

        var settingsVm = new SettingsViewModel(mockAppConfig.Object, mockThemeSelectorService.Object, mockSystemService.Object, mockApplicationInfoService.Object);
        settingsVm.SetThemeCommand.Execute(AppTheme.Light.ToString());

        mockThemeSelectorService.Verify(mock => mock.SetTheme(AppTheme.Light));
    }
}
