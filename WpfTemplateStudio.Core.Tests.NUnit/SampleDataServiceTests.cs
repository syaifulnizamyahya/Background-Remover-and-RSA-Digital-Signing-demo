using NUnit.Framework;

using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.Core.Tests.NUnit;

public class SampleDataServiceTests
{
    public SampleDataServiceTests()
    {
    }

    // Remove or update this once your app is using real data and not the SampleDataService.
    // This test serves only as a demonstration of testing functionality in the Core library.
    [Test]
    public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
    {
        var sampleDataService = new SampleDataService();

        var data = await sampleDataService.GetContentGridDataAsync();

        Assert.That(data, Is.Not.Null);
        //Assert.IsTrue(data.Any());
    }

    // Remove or update this once your app is using real data and not the SampleDataService.
    // This test serves only as a demonstration of testing functionality in the Core library.
    [Test]
    public async Task EnsureSampleDataServiceReturnsGridDataAsync()
    {
        var sampleDataService = new SampleDataService();

        var data = await sampleDataService.GetGridDataAsync();

        Assert.That(data, Is.Not.Null);
        //Assert.IsTrue(data.Any());
    }

    // Remove or update this once your app is using real data and not the SampleDataService.
    // This test serves only as a demonstration of testing functionality in the Core library.
    [Test]
    public async Task EnsureSampleDataServiceReturnsListDetailsDataAsync()
    {
        var sampleDataService = new SampleDataService();

        var data = await sampleDataService.GetListDetailsDataAsync();

        Assert.That(data, Is.Not.Null);
        //Assert.IsTrue(data.Any());
    }
}
