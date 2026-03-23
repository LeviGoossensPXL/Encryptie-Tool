using System.Security.Cryptography;
using WebApplication1.Entities;
using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces
{
    public interface IAesEncryptionService
    {
        public AesEncryptionResult Encrypt(string plaintext, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode);

        public AesDecryptionResult Decrypt(string ciphertext, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode);
    }
}
