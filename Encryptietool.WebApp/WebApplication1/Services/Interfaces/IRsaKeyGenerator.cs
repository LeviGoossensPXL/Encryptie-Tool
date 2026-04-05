using WebApplication1.Models;
using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces
{
    public interface IRsaKeyGenerator
    {
        RsaKeyResult GenerateKeys(int keySize);
    }
}