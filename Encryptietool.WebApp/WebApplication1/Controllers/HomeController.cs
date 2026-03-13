using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAesEncryptionService _aesEncryptionService;

        public HomeController(ILogger<HomeController> logger, IAesEncryptionService aesEncryptionService)
        {
            _logger = logger;
            _aesEncryptionService = aesEncryptionService;
        }

        public IActionResult Index()
        {
            string plaintext = "hello world this is a new place for friends";
            Aes aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
            EncryptionResult result = _aesEncryptionService.Encrypt(plaintext, aes.Key, aes.IV, CipherMode.CBC, PaddingMode.None);

            string output = _aesEncryptionService.Decrypt(result.Ciphertext, aes.Key, aes.IV, CipherMode.CBC, PaddingMode.None);
            EncryptionViewModel encryptionViewModel = new()
            {
                InputText = plaintext,
                CipherText = Convert.ToBase64String(result.Ciphertext),
                OutputText = output
            };

            return View(encryptionViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
