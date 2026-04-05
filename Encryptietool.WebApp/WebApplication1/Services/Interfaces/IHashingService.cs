using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IHashingService
    {
        string ComputeHash(string input, HashAlgorithmType type);
        string ComputeFileHash(Stream fileStream, HashAlgorithmType type);

        bool VerifyHash(string input, string expectedHash, HashAlgorithmType type);
        bool VerifyFileHash(Stream fileStream, string expectedHash, HashAlgorithmType type);

        string ComputeHmac(string input, string secretKey, HmacAlgorithmType type);
        string ComputeFileHmac(Stream fileStream, string secretKey, HmacAlgorithmType type);

        bool VerifyHmac(string input, string expectedHmac, string secretKey, HmacAlgorithmType type);
        bool VerifyFileHmac(Stream fileStream, string expectedHmac, string secretKey, HmacAlgorithmType type);
    }
}
