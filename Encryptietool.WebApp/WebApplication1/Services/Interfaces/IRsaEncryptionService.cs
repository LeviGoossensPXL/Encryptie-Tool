using System.Security.Cryptography;

namespace WebApplication1.Services.Interfaces;

public interface IRsaEncryptionService
{
    public byte[] EncryptKey(byte[] aesKey, RSA publicKey);
    public byte[] DecryptKey(byte[] encryptedKey, RSA privateKey);
    public byte[] SignData(byte[] data, RSA privateKey);
    public bool VerifySignature(byte[] data, byte[] signature, RSA publicKey);
}