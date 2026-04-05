using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class HashingService : IHashingService
    {
        public string ComputeHash(string input, HashAlgorithmType type)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input ?? string.Empty);

            using var algorithm = CreateHashAlgorithm(type);
            byte[] hashBytes = algorithm.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        public string ComputeFileHash(Stream fileStream, HashAlgorithmType type)
        {
            using var algorithm = CreateHashAlgorithm(type);
            byte[] hashBytes = algorithm.ComputeHash(fileStream);

            return Convert.ToHexString(hashBytes);
        }

        public bool VerifyHash(string input, string expectedHash, HashAlgorithmType type)
        {
            string computedHash = ComputeHash(input, type);
            string normalizedExpectedHash = NormalizeHex(expectedHash);

            return string.Equals(computedHash, normalizedExpectedHash, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyFileHash(Stream fileStream, string expectedHash, HashAlgorithmType type)
        {
            string computedHash = ComputeFileHash(fileStream, type);
            string normalizedExpectedHash = NormalizeHex(expectedHash);

            return string.Equals(computedHash, normalizedExpectedHash, StringComparison.OrdinalIgnoreCase);
        }

        public string ComputeHmac(string input, string secretKey, HmacAlgorithmType type)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input ?? string.Empty);
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey ?? string.Empty);

            using var hmac = CreateHmacAlgorithm(type, keyBytes);
            byte[] hmacBytes = hmac.ComputeHash(inputBytes);

            return Convert.ToHexString(hmacBytes);
        }

        public string ComputeFileHmac(Stream fileStream, string secretKey, HmacAlgorithmType type)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey ?? string.Empty);

            using var hmac = CreateHmacAlgorithm(type, keyBytes);
            byte[] hmacBytes = hmac.ComputeHash(fileStream);

            return Convert.ToHexString(hmacBytes);
        }

        public bool VerifyHmac(string input, string expectedHmac, string secretKey, HmacAlgorithmType type)
        {
            string computedHmac = ComputeHmac(input, secretKey, type);
            string normalizedExpectedHmac = NormalizeHex(expectedHmac);

            return string.Equals(computedHmac, normalizedExpectedHmac, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyFileHmac(Stream fileStream, string expectedHmac, string secretKey, HmacAlgorithmType type)
        {
            string computedHmac = ComputeFileHmac(fileStream, secretKey, type);
            string normalizedExpectedHmac = NormalizeHex(expectedHmac);

            return string.Equals(computedHmac, normalizedExpectedHmac, StringComparison.OrdinalIgnoreCase);
        }

        private static HashAlgorithm CreateHashAlgorithm(HashAlgorithmType type)
        {
            return type switch
            {
                HashAlgorithmType.MD5 => MD5.Create(),
                HashAlgorithmType.SHA1 => SHA1.Create(),
                HashAlgorithmType.SHA256 => SHA256.Create(),
                HashAlgorithmType.SHA384 => SHA384.Create(),
                HashAlgorithmType.SHA512 => SHA512.Create(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported hash algorithm.")
            };
        }

        private static HMAC CreateHmacAlgorithm(HmacAlgorithmType type, byte[] key)
        {
            return type switch
            {
                HmacAlgorithmType.HMACSHA256 => new HMACSHA256(key),
                HmacAlgorithmType.HMACSHA384 => new HMACSHA384(key),
                HmacAlgorithmType.HMACSHA512 => new HMACSHA512(key),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported HMAC algorithm.")
            };
        }

        private static string NormalizeHex(string? input)
        {
            return (input ?? string.Empty)
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .Trim();
        }
    }
}
