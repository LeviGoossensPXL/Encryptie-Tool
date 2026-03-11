using System.Security.Cryptography;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IAesEncryptionService
    {
        public EncryptionResult Encrypt(string plaintext, byte[] key, byte[] iv, CipherMode cipherMode,
            PaddingMode paddingMode);

        public string Decrypt(byte[] ciphertext, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode);
    }
}
