using System.Security.Cryptography;
using WebApplication1.Entities;
using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces
{
    public interface IAesEncryptionService
    {
        public AesEncryptionResult Encrypt(string plaintext, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode);

        public AesDecryptionResult Decrypt(byte[] ciphertext, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode);
    }
}
