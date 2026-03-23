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

        // public IActionResult Index()
        // {
        //     return View();
        // }

        public IActionResult Encryption()
        {
            return View(new AesEncryptionViewModel());
        }

        [HttpPost]
        public IActionResult Encryption(AesEncryptionViewModel aesEncryptionViewModel)
        {
            Aes aes = Aes.Create();
            AesEncryptionResult result = _aesEncryptionService.Encrypt(aesEncryptionViewModel.InputText, aes.Key, aes.IV, CipherMode.CBC, PaddingMode.PKCS7);
            aesEncryptionViewModel.OutputText = Convert.ToBase64String(result.Ciphertext);
            aesEncryptionViewModel.IV = Convert.ToBase64String(aes.IV);
            aesEncryptionViewModel.Key = Convert.ToBase64String(aes.Key);
            return View(aesEncryptionViewModel);
        }
        
        public IActionResult Decryption()
        {
            return View(new AesDecryptionViewModel());
        }
        
        [HttpPost]
        public IActionResult Decryption(AesDecryptionViewModel aesDecryptionViewModel)
        {
            Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(aesDecryptionViewModel.Key);
            aes.IV = Convert.FromBase64String(aesDecryptionViewModel.IV);
            AesDecryptionResult result = _aesEncryptionService.Decrypt(Convert.FromBase64String(aesDecryptionViewModel.InputText), aes.Key, aes.IV, CipherMode.CBC, PaddingMode.PKCS7);
            aesDecryptionViewModel.OutputText = result.DecryptedText;
            return View(aesDecryptionViewModel);
        }
    }
}
