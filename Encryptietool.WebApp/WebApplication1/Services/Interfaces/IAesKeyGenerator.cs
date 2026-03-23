using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IAesKeyGenerator
    {
        AesKeyResult GenerateKey(int keySize);
    }
}