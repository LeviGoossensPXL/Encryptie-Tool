using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IRsaKeyGenerator
    {
        RsaKeyResult GenerateKeys(int keySize);
    }
}