using System.IO;

using Newtonsoft.Json;

using NUnit.Framework;

using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.Core.Tests.NUnit;

public class FileServiceTests
{
    private string _folderPath;
    private string _fileName;
    private string _fileData;
    private string _filePath;

    public FileServiceTests()
    {
    }

    [SetUp]
    public void Setup()
    {
        _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UnitTests");
        _fileName = "Tests.json";
        _fileData = "Lorem ipsum dolor sit amet";
        _filePath = Path.Combine(_folderPath, _fileName);
    }

    [Test]
    public void TestSaveFile()
    {
        var fileService = new FileService();

        fileService.Save(_folderPath, _fileName, _fileData);

        if (File.Exists(_filePath))
        {
            var jsonContentFile = File.ReadAllText(_filePath);
            var contentFile = JsonConvert.DeserializeObject<string>(jsonContentFile);
            Assert.That(contentFile, Is.EqualTo(_fileData));
            //Assert.AreEqual(_fileData, contentFile);
        }
        else
        {
            Assert.Fail($"File not exist: {_filePath}");
        }
    }

    [Test]
    public void TestReadFile()
    {
        var fileService = new FileService();
        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_fileData));

        var cacheData = fileService.Read<string>(_folderPath, _fileName);

        Assert.That(cacheData, Is.EqualTo(_fileData));
        //Assert.AreEqual(_fileData, cacheData);
    }

    [Test]
    public void TestDeleteFile()
    {
        var fileService = new FileService();
        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        File.WriteAllText(_filePath, _fileData);

        fileService.Delete(_folderPath, _fileName);

        Assert.That(!File.Exists(_filePath));
        //Assert.IsFalse(File.Exists(_filePath));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}
