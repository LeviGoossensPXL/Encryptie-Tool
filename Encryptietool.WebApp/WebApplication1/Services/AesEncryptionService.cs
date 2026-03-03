using System.Security.Cryptography;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AesEncryptionService
    {
        public EncryptionResult Encrypt(string plaintext, byte[] key, byte[] iv, CipherMode mode)
        {
            // Implementeer encryptie
            // Gebruik AES met gekozen mode
            // Return EncryptionResult met ciphertext en metadata
            return new EncryptionResult();
        }

        public string Decrypt(byte[] ciphertext, byte[] key, byte[] iv, CipherMode mode)
        {
            // Implementeer decryptie
            // Handle exceptions (wrong key, corrupted data)
            // Return plaintext
            return "";
        }
    }
}
