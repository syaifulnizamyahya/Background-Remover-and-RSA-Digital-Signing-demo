using NUnit.Framework;

namespace WpfTemplateStudio.Core.Services.Tests
{
    [TestFixture()]
    public class BackgroundRemoverServiceTests
    {
        [TestCase("testImage.jpg", null, null, "testImage.png")]
        [TestCase("testImage.jpg", "", null, "testImage.png")]//
        [TestCase("testImage.jpg", null, BackgroundRemoverService.DeepLearningModel.u2net, "testImage_u2net.png")]
        [TestCase("testImage.jpg", "testImage.png", null, "testImage.png")]
        [TestCase("testImage.jpg", "testImage_u2net.png", BackgroundRemoverService.DeepLearningModel.u2net, "testImage_u2net.png")]
        [TestCase("testImage.jpg", "testImage_u2netp.png", BackgroundRemoverService.DeepLearningModel.u2netp, "testImage_u2netp.png")]
        [TestCase("testImage.jpg", "testImage_u2net_human_seg.png", BackgroundRemoverService.DeepLearningModel.u2net_human_seg, "testImage_u2net_human_seg.png")]
        [TestCase("testImage.jpg", "testImage_silueta.png", BackgroundRemoverService.DeepLearningModel.silueta, "testImage_silueta.png")]
        [TestCase("testImage.jpg", "testImage_isnet_general_use.png", BackgroundRemoverService.DeepLearningModel.isnet_general_use, "testImage_isnet_general_use.png")]
        public void RemoveBackground_GivenModelAndOutput_CreatedOutputFileAndReturnBytes(
            string input
            , string output
            , BackgroundRemoverService.DeepLearningModel model
            , string expectedResult
            )
        {
            // Arrange
            var currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            var inputAssetPath = Path.Combine(currentDirectory, "Assets/Input/", input);
            var outputAssetPath = String.Empty;
            if (!String.IsNullOrEmpty(output))
            {
                outputAssetPath = Path.Combine(currentDirectory, "Assets/Output/", output);
            }
            var outputReferenceAssetPath = Path.Combine(currentDirectory, "Assets/OutputReference/", expectedResult);
            var expectedResultBytes = File.ReadAllBytes(outputReferenceAssetPath);

            // Act
            var resultBytes = BackgroundRemoverService.RemoveBackground(inputAssetPath, outputAssetPath, model);
            byte[] resultFileBytes = null;
            if (!String.IsNullOrEmpty(output))
            {
                File.WriteAllBytes(outputAssetPath, resultBytes);
                resultFileBytes = File.ReadAllBytes(outputAssetPath);
            }

            // Assert
            Assert.That(resultBytes, Is.EqualTo(expectedResultBytes));
            if (resultFileBytes != null)
            {
                Assert.That(resultFileBytes, Is.EqualTo(expectedResultBytes));
            }
        }
    }
}