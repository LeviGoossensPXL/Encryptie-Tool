using System.Security.Cryptography;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class RsaEncryptionService : IRsaEncryptionService
{
    public byte[] EncryptKey(byte[] aesKey, RSA publicKey)
    {
        return publicKey.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
    }

    public byte[] DecryptKey(byte[] encryptedKey, RSA privateKey)
    {
        return privateKey.Decrypt(encryptedKey, RSAEncryptionPadding.OaepSHA256);
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