using System.Security.Cryptography;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class RsaEncryptionService : IRsaEncryptionService
{
    public byte[] EncryptKey(byte[] aesKey, RSA publicKey)
    {
        throw new NotImplementedException();
    }

    public byte[] DecryptKey(byte[] encryptedKey, RSA privateKey)
    {
        throw new NotImplementedException();
    }

    public byte[] SignData(byte[] data, RSA privateKey)
    {
        throw new NotImplementedException();
    }

    public bool VerifySignature(byte[] data, byte[] signature, RSA publicKey)
    {
        throw new NotImplementedException();
    }
}