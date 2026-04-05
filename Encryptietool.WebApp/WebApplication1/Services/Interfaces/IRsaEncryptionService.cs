using System.Security.Cryptography;
using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces;

public interface IRsaEncryptionService
{
    public RsaEncryptionResult EncryptKey(string aesKey, string publicKey);
    public RsaDecryptionResult DecryptKey(string input, string privateKey);
    public byte[] SignData(byte[] data, RSA privateKey);
    public bool VerifySignature(byte[] data, byte[] signature, RSA publicKey);
}