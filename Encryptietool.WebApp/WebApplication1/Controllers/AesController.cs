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
            Aes aes = Aes.Create();
            var aesEncryptionViewModel = new AesEncryptionViewModel
            {
                InputText = "Hello World! ;-)",
                Key = Convert.ToBase64String(aes.Key),
                IV = Convert.ToBase64String(aes.IV)
            };
            return View(aesEncryptionViewModel);
        }

        [HttpPost]
        public IActionResult Encryption(AesEncryptionViewModel aesEncryptionViewModel)
        {
            var cipherMode = Enum.Parse<CipherMode>(aesEncryptionViewModel.CipherMode);
            var paddingMode = Enum.Parse<PaddingMode>(aesEncryptionViewModel.PaddingMode);
            
            AesEncryptionResult result = _aesEncryptionService.Encrypt(aesEncryptionViewModel.InputText, aesEncryptionViewModel.Key, aesEncryptionViewModel.IV, cipherMode, paddingMode);
            aesEncryptionViewModel.OutputText = result.Ciphertext;
            
            return View(aesEncryptionViewModel);
        }
        
        public IActionResult Decryption()
        {
            return View(new AesDecryptionViewModel());
        }
        
        [HttpPost]
        public IActionResult Decryption(AesDecryptionViewModel aesDecryptionViewModel)
        {
            var cipherMode = Enum.Parse<CipherMode>(aesDecryptionViewModel.CipherMode);
            var paddingMode = Enum.Parse<PaddingMode>(aesDecryptionViewModel.PaddingMode);
            
            AesDecryptionResult result = _aesEncryptionService.Decrypt(aesDecryptionViewModel.InputText, aesDecryptionViewModel.Key, aesDecryptionViewModel.IV, cipherMode, paddingMode);
            aesDecryptionViewModel.OutputText = result.DecryptedText;
            
            return View(aesDecryptionViewModel);
        }
    }
}
