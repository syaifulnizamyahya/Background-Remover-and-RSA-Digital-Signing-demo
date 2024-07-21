namespace WpfTemplateStudio.Core.Contracts.Services;

public interface IRsaDigitalSignService
{
    static void RsaKeyGenerator(ref string publicKey, ref string privateKey) => throw new NotImplementedException();
    static byte[] SignData(object dataToSign, string privateKey) => throw new NotImplementedException();
    static bool VerifyData(object dataToVerify, string publicKey, byte[] signature) => throw new NotImplementedException();
}
