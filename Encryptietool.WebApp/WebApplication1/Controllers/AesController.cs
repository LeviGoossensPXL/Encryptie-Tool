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
            AesEncryptionResult result = _aesEncryptionService.Encrypt(encryptionViewModel.InputText, aes.Key, aes.IV, CipherMode.CBC, PaddingMode.PKCS7);
            encryptionViewModel.OutputText = Convert.ToBase64String(result.Ciphertext);
            encryptionViewModel.IV = Convert.ToBase64String(aes.IV);
            encryptionViewModel.Key = Convert.ToBase64String(aes.Key);
            return View("Encrypt", encryptionViewModel);
        }
        
        public IActionResult Decryption()
        {
            return View(new DecryptionViewModel());
        }
        
        [HttpPost]
        public IActionResult Decryption(DecryptionViewModel decryptionViewModel)
        {
            Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(decryptionViewModel.Key);
            aes.IV = Convert.FromBase64String(decryptionViewModel.IV);
            AesDecryptionResult result = _aesEncryptionService.Decrypt(Convert.FromBase64String(decryptionViewModel.InputText), aes.Key, aes.IV, CipherMode.CBC, PaddingMode.PKCS7);
            ViewBag.OutputText = result.DecryptedText;
            return View(decryptionViewModel);
        }
    }
}
