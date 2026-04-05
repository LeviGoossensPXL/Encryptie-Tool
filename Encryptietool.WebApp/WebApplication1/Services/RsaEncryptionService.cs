using System.Security.Cryptography;
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class RsaEncryptionService : IRsaEncryptionService
{
    public RsaEncryptionResult EncryptKey(string input, string publicKey)
    {
        var result = new RsaEncryptionResult();
        using var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(publicKey);
        }
        catch (ArgumentException)
        {
            result.Failed("Invalid PEM key.");
            return result;
        }
        
        byte[] inputBytes;
        try
        {
            inputBytes = Convert.FromBase64String(input);
        }
        catch (Exception)
        {
            result.Failed("Invalid input format. Expected base64 string");
            return result;
        }

        var cipher = rsa.Encrypt(inputBytes, RSAEncryptionPadding.OaepSHA256);
        result.CipherText = Convert.ToBase64String(cipher);
        return result;
    }

    public RsaDecryptionResult DecryptKey(string input, string privateKey)
    {
        var result = new RsaDecryptionResult();
        using var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(privateKey);
        }
        catch (Exception)
        {
            result.Failed("Invalid PEM key.");
            return result;
        }

        byte[] inputBytes;
        try
        {
             inputBytes = Convert.FromBase64String(input);
        }
        catch (Exception)
        {
            result.Failed("Invalid input format. Expected base64 string");
            return result;
        }
        
        var cipher = rsa.Decrypt(inputBytes, RSAEncryptionPadding.OaepSHA256);
        result.PlainText = Convert.ToBase64String(cipher);
        return result;
    }

    public byte[] SignData(byte[] data, RSA privateKey)
    {
        return privateKey.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public bool VerifySignature(byte[] data, byte[] signature, RSA publicKey)
    {
        return publicKey.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}