using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
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

        public AesController(ILogger<AesController> logger, IAesEncryptionService aesEncryptionService,
            IFileService fileService)
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
            // Aes aes = Aes.Create();
            // var aesEncryptionViewModel = new AesEncryptionViewModel
            // {
            //     InputText = "Hello World! ;-)",
            //     Key = Convert.ToBase64String(aes.Key),
            //     IV = Convert.ToBase64String(aes.IV)
            // };
            var aesEncryptionViewModel = new AesEncryptionViewModel();
            return View(aesEncryptionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Encryption(AesEncryptionViewModel aesEncryptionViewModel)
        {
            var cipherMode = Enum.Parse<CipherMode>(aesEncryptionViewModel.CipherMode);
            var paddingMode = Enum.Parse<PaddingMode>(aesEncryptionViewModel.PaddingMode);
            AesEncryptionResult aesEncryptionResult;
            if (aesEncryptionViewModel.IsFileUpload)
            {
                var fileSaveResult = await _fileService.Save(aesEncryptionViewModel.InputFile);
                if (!fileSaveResult.Succeeded)
                {
                    return View(aesEncryptionViewModel);
                }

                aesEncryptionResult = _aesEncryptionService.EncryptFile(fileSaveResult.FileInfo,
                    aesEncryptionViewModel.Key, aesEncryptionViewModel.IV, cipherMode, paddingMode);
                aesEncryptionViewModel.OutputFile = aesEncryptionResult.FileInfo.Name;
                return View(aesEncryptionViewModel);
            }

            aesEncryptionResult = _aesEncryptionService.Encrypt(aesEncryptionViewModel.InputText,
                aesEncryptionViewModel.Key, aesEncryptionViewModel.IV, cipherMode, paddingMode);
            aesEncryptionViewModel.OutputText = aesEncryptionResult.EncryptedText;

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
            AesDecryptionResult aesDecryptionResult;
            if (aesDecryptionViewModel.IsFileUpload)
            {
                var fileSaveResult = await _fileService.Save(aesDecryptionViewModel.InputFile);
                if (!fileSaveResult.Succeeded)
                {
                    return View(aesDecryptionViewModel);
                }

                aesDecryptionResult = _aesEncryptionService.DecryptFile(fileSaveResult.FileInfo,
                    aesDecryptionViewModel.Key, aesDecryptionViewModel.IV, cipherMode, paddingMode);
                aesDecryptionViewModel.OutputFile = aesDecryptionResult.FileInfo.Name;
                return View(aesDecryptionViewModel);
            }

            aesDecryptionResult = _aesEncryptionService.Decrypt(aesDecryptionViewModel.InputText,
                aesDecryptionViewModel.Key, aesDecryptionViewModel.IV, cipherMode, paddingMode);
            aesDecryptionViewModel.OutputText = aesDecryptionResult.DecryptedText;

            return View(aesDecryptionViewModel);
        }

        [HttpGet]
        public IActionResult Download(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return Content("Filename is not provided.");
            }
            var fileDownloadResult = _fileService.Download(filename);
            if (!fileDownloadResult.Succeeded)
            {
                return Content("File not found.");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileDownloadResult.FileInfo.FullName);
            
            return File(fileBytes, "application/octet-stream", filename);
        }
    }
}
