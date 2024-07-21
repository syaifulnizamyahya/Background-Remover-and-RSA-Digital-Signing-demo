using System.IO;

using Newtonsoft.Json;

using NUnit.Framework;

using WpfTemplateStudio.Core.Services;

namespace WpfTemplateStudio.Core.Tests.NUnit;

public class RsaDigitalSignServiceTests
{
    [TestCase("This is the data to be signed.", "This is the data to be signed.", true)]
    [TestCase("This is the data to be signed.", "This is NOT the data to be signed.", false)]
    public void SignAndVerify_ShouldReturnTrueOrFalse(string dataToSign, string dataToVerify, bool expectedResult)
    {
        //Arrange 
        string privateKey = null;
        string publicKey = null;
        RsaDigitalSignService.RsaKeyGenerator(ref publicKey, ref privateKey);

        //Act
        var signedData = RsaDigitalSignService.SignData(dataToSign, privateKey);
        bool result = RsaDigitalSignService.VerifyData(dataToVerify, publicKey, signedData);

        //Assert
        Assert.AreEqual(expectedResult, result);
    }
}
