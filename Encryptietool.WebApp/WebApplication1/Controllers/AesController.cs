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
        private readonly IFileService _fileService;

        public AesController(ILogger<AesController> logger, IAesEncryptionService aesEncryptionService, IFileService fileService)
        {
            _logger = logger;
            _aesEncryptionService = aesEncryptionService;
            _fileService = fileService;
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
        public async Task<IActionResult> Encryption(AesEncryptionViewModel aesEncryptionViewModel)
        {
            var cipherMode = Enum.Parse<CipherMode>(aesEncryptionViewModel.CipherMode);
            var paddingMode = Enum.Parse<PaddingMode>(aesEncryptionViewModel.PaddingMode);
            if (aesEncryptionViewModel.IsFileUpload)
            {
                var fileSaveResult = await _fileService.Save(aesEncryptionViewModel.InputFile);
                if (!fileSaveResult.Succeeded)
                {
                    return View(aesEncryptionViewModel);
                }
                var aesEncryptionResult = _aesEncryptionService.EncryptFile(fileSaveResult.FileInfo, aesEncryptionViewModel.Key, aesEncryptionViewModel.IV, cipherMode, paddingMode);
                return View(aesEncryptionViewModel);
            }
            AesEncryptionResult result = _aesEncryptionService.Encrypt(aesEncryptionViewModel.InputText, aesEncryptionViewModel.Key, aesEncryptionViewModel.IV, cipherMode, paddingMode);
            aesEncryptionViewModel.OutputText = result.Ciphertext;
            
            return View(aesEncryptionViewModel);
        }
        
        public IActionResult Decryption()
        {
            return View(new AesDecryptionViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Decryption(AesDecryptionViewModel aesDecryptionViewModel)
        {
            var cipherMode = Enum.Parse<CipherMode>(aesDecryptionViewModel.CipherMode);
            var paddingMode = Enum.Parse<PaddingMode>(aesDecryptionViewModel.PaddingMode);
            if (aesDecryptionViewModel.IsFileUpload)
            {
                var fileSaveResult = await _fileService.Save(aesDecryptionViewModel.InputFile);
                if (!fileSaveResult.Succeeded)
                {
                    return View(aesDecryptionViewModel);
                }
                var aesDecryptionResult = _aesEncryptionService.DecryptFile(fileSaveResult.FileInfo, aesDecryptionViewModel.Key, aesDecryptionViewModel.IV, cipherMode, paddingMode);
                return View(aesDecryptionViewModel);
            }
            AesDecryptionResult result = _aesEncryptionService.Decrypt(aesDecryptionViewModel.InputText, aesDecryptionViewModel.Key, aesDecryptionViewModel.IV, cipherMode, paddingMode);
            aesDecryptionViewModel.OutputText = result.DecryptedText;
            
            return View(aesDecryptionViewModel);
        }
    }
}
