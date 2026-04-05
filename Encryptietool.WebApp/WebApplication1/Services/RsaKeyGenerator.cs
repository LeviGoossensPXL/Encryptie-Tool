using System.Security.Cryptography;
using WebApplication1.Models;
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class RsaKeyGenerator : IRsaKeyGenerator
    {
        public RsaKeyResult GenerateKeys(int keySize)
        {
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new ArgumentException("RSA key size must be 1024, 2048, or 4096 bits.");
            }

            using RSA rsa = RSA.Create(keySize);

            return new RsaKeyResult
            {
                PublicKeyPem = rsa.ExportRSAPublicKeyPem(),
                PrivateKeyPem = rsa.ExportRSAPrivateKeyPem(),
                KeySize = keySize
            };
        }
    }
}