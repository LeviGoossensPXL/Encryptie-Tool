using System.Security.Cryptography;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AesKeyGenerator : IAesKeyGenerator
    {
        public AesKeyResult GenerateKey(int keySize)
        {
            if (keySize != 128 && keySize != 192 && keySize != 256)
            {
                throw new ArgumentException("AES key size must be 128, 192, or 256 bits.");
            }

            using Aes aes = Aes.Create();
            aes.KeySize = keySize;
            aes.GenerateKey();
            aes.GenerateIV();

            return new AesKeyResult
            {
                KeyBytes = aes.Key,
                IVBytes = aes.IV,
                KeyBase64 = Convert.ToBase64String(aes.Key),
                KeyHex = Convert.ToHexString(aes.Key),
                IVBase64 = Convert.ToBase64String(aes.IV),
                IVHex = Convert.ToHexString(aes.IV),
                KeySize = keySize
            };
        }
    }
}