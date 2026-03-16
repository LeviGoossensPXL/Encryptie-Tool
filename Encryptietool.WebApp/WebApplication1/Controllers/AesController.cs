using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class AesController : Controller
    {
        private readonly ILogger<AesController> _logger;
        private readonly IAesEncryptionService _aesEncryptionService;

        public AesController(ILogger<AesController> logger, IAesEncryptionService aesEncryptionService)
        {
            _logger = logger;
            _aesEncryptionService = aesEncryptionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Encrypt()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EncryptPost()
        {
            Aes aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
            EncryptionResult result = _aesEncryptionService.Encrypt("hello", aes.Key, aes.IV,
                CipherMode.CBC, PaddingMode.PKCS7);
            _logger.Log(LogLevel.Information,Convert.ToBase64String(result.Ciphertext));
            return View("Encrypt");
        }
    }
}
