using NUnit.Framework;

namespace WpfTemplateStudio.Core.Services.Tests
{
    [TestFixture()]
    public class BackgroundRemoverServiceTests
    {
        [TestCase("testImage.jpg", "testImage.png", "testImage.png")]
        public void RemoveBackground_CreatedOutputFileAndReturnBytes(string input, string output, string expectedResult)
        {
            // Arrange
            var currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            var inputAssetPath = Path.Combine(currentDirectory, "Assets/Input/", input);
            var outputAssetPath = Path.Combine(currentDirectory, "Assets/Output/", output);
            var outputReferenceAssetPath = Path.Combine(currentDirectory, "Assets/OutputReference/", expectedResult);
            var expectedResultBytes = File.ReadAllBytes(outputReferenceAssetPath);

            // Act
            var resultBytes = BackgroundRemoverService.RemoveBackground(inputAssetPath, outputAssetPath);
            var resultFileBytes = File.ReadAllBytes(outputAssetPath);

            // Assert
            Assert.That(resultBytes, Is.EqualTo(expectedResultBytes));
            Assert.That(resultFileBytes, Is.EqualTo(expectedResultBytes));
        }

        [TestCase("testImage.jpg", "testImage.png")]
        public void RemoveBackground_ReturnBytes(string input, string expectedResult)
        {
            // Arrange
            var currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            var inputAssetPath = Path.Combine(currentDirectory, "Assets/Input/", input);
            var outputReferenceAssetPath = Path.Combine(currentDirectory, "Assets/OutputReference/", expectedResult);
            var expectedResultBytes = File.ReadAllBytes(outputReferenceAssetPath);

            // Act
            var resultBytes = BackgroundRemoverService.RemoveBackground(inputAssetPath);

            // Assert
            Assert.That(resultBytes, Is.EqualTo(expectedResultBytes));
        }
    }
}