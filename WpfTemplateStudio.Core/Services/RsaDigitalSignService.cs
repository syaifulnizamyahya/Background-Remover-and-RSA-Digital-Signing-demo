using System.Security.Cryptography;

using WpfTemplateStudio.Core.Contracts.Services;

namespace WpfTemplateStudio.Core.Services;

public class RsaDigitalSignService : IRsaDigitalSignService
{
    public static void RsaKeyGenerator(ref string publicKey, ref string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            // Export the public key
            publicKey = rsa.ToXmlString(false);
            // Export the private key
            privateKey = rsa.ToXmlString(true);
        }
    }

    public static byte[] SignData(object dataToSign, string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);

            // Convert data to bytes
            var dataBytes = ObjectSerializer.ObjectToByteArray(dataToSign);
            var hash = SHA256.Create().ComputeHash(dataBytes);

            var signedBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

            return signedBytes;
        }
    }

    public static bool VerifyData(object dataToVerify, string publicKey, byte[] signature)
    {
        if (dataToVerify == null || publicKey == null || signature == null)
        {
            var errorMessage = "Data to verify : " + dataToVerify + ", public key : " + publicKey + ", or signature : " + signature + " cannot be null.";
            throw new ArgumentNullException(errorMessage);
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);

            // Convert data to bytes
            var dataBytes = ObjectSerializer.ObjectToByteArray(dataToVerify);
            var hash = SHA256.Create().ComputeHash(dataBytes);

            var oid = CryptoConfig.MapNameToOID("SHA256");

            if (oid != null)
            {
                return rsa.VerifyHash(hash, oid, signature);
            }
            else
            {
                // Handle the case when CryptoConfig.MapNameToOID("SHA256") returns null
                // Maybe throw an exception or handle it in a way that makes sense for your application
                return false;
            }
        }
    }
}
