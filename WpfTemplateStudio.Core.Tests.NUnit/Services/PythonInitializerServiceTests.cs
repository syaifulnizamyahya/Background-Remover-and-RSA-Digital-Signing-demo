using NUnit.Framework;
using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.Core.Tests.NUnit.Services;

[TestFixture()]
public class PythonInitializerServiceTests
{
    [Test()]
    public void Instance_IsNotNull()
    {
        // Act
        var instance = PythonInitializerService.Instance;

        // Assert
        Assert.That(instance, Is.Not.Null);
    }

    [Test()]
    public void Instance_IsSameInstance()
    {
        // Act
        var instance1 = PythonInitializerService.Instance;
        var instance2 = PythonInitializerService.Instance;

        // Assert
        Assert.That(instance1, Is.SameAs(instance2));
    }

    [Test()]
    public void Initialized_ReturnsExpectedVersion()
    {
        // Arrange
        var expectedMessage = "Initialized Python version: 3.12.4 | packaged by Anaconda, Inc. | (main, Jun 18 2024, 15:03:56) [MSC v.1929 64 bit (AMD64)]";

        // Act
        var message = PythonInitializerService.Instance.Initialize();

        // Assert
        Assert.That(message, Is.EqualTo(expectedMessage));
    }
}

