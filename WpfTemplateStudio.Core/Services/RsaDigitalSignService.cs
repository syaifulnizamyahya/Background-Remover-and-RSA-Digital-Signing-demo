using System.Security.Cryptography;

using WpfTemplateStudio.Core.Contracts.Services;

namespace WpfTemplateStudio.Core.Services;

public class RsaDigitalSignService : IRsaDigitalSignService
{
    /// <summary>
    /// Generates public and private keys
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="privateKey"></param>
    public static void RsaKeyGenerator(ref string publicKey, ref string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            publicKey = rsa.ToXmlString(false);
            privateKey = rsa.ToXmlString(true);
        }
    }

    /// <summary>
    /// Generate digital signature of data
    /// </summary>
    /// <param name="dataToSign"></param>
    /// <param name="privateKey"></param>
    /// <returns></returns>
    public static byte[] SignData(object dataToSign, string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);

            var dataBytes = ObjectSerializer.ObjectToByteArray(dataToSign);
            var hash = SHA256.Create().ComputeHash(dataBytes);

            var signedBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

            return signedBytes;
        }
    }

    /// <summary>
    /// Verify digital signature
    /// </summary>
    /// <param name="dataToVerify"></param>
    /// <param name="publicKey"></param>
    /// <param name="signature"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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

            var dataBytes = ObjectSerializer.ObjectToByteArray(dataToVerify);
            var hash = SHA256.Create().ComputeHash(dataBytes);

            var oid = CryptoConfig.MapNameToOID("SHA256");

            if (oid != null)
            {
                return rsa.VerifyHash(hash, oid, signature);
            }
            else
            {
                return false;
            }
        }
    }
}
