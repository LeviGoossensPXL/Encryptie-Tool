using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;
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
            return View(new EncryptionViewModel());
        }

        [HttpPost]
        public IActionResult Encrypt(EncryptionViewModel encryptionViewModel)
        {
            Aes aes = Aes.Create();
            AesEncryptionResult result = _aesEncryptionService.Encrypt("hello", aes.Key, aes.IV, CipherMode.CBC, PaddingMode.PKCS7);
            ViewBag.OutputText = Convert.ToBase64String(result.Ciphertext);
            return View("Encrypt", encryptionViewModel);
        }
    }
}
