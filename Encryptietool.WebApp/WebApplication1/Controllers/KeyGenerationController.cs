using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class KeyGenerationController : Controller
    {
        private readonly IAesKeyGenerator _aesKeyGenerator;
        private readonly IRsaKeyGenerator _rsaKeyGenerator;
        private readonly ILogger<KeyGenerationController> _logger;

        public KeyGenerationController(
            IAesKeyGenerator aesKeyGenerator,
            IRsaKeyGenerator rsaKeyGenerator,
            ILogger<KeyGenerationController> logger)
        {
            _aesKeyGenerator = aesKeyGenerator;
            _rsaKeyGenerator = rsaKeyGenerator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new KeyGenerationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(KeyGenerationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.KeyType == "AES")
                {
                    model.AesResult = _aesKeyGenerator.GenerateKey(model.KeySize);
                    model.RsaResult = null;
                }
                else if (model.KeyType == "RSA")
                {
                    model.RsaResult = _rsaKeyGenerator.GenerateKeys(model.KeySize);
                    model.AesResult = null;
                }
                else
                {
                    model.ErrorMessage = "Invalid key type selected.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating keys.");
                model.ErrorMessage = "Er ging iets mis bij het genereren van de sleutel.";
            }

            return View(model);
        }





        [HttpPost]
        public IActionResult DownloadAesKey(string key)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(key);
            return File(bytes, "application/octet-stream", "aes-key.txt");
        }

        [HttpPost]
        public IActionResult DownloadAesIv(string iv)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(iv);
            return File(bytes, "application/octet-stream", "aes-iv.txt");
        }

        [HttpPost]
        public IActionResult DownloadAesCombined(string key, string iv)
        {
            var content = $"KEY:\n{key}\n\nIV:\n{iv}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            return File(bytes, "application/octet-stream", "aes-key-iv.txt");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DownloadRsaPublicKey(string publicKeyPem)
        {
            if (string.IsNullOrWhiteSpace(publicKeyPem))
            {
                TempData["ErrorMessage"] = "No public key available for download.";
                return RedirectToAction(nameof(Index));
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(publicKeyPem);
            return File(bytes, "application/x-pem-file", "rsa-public-key.pem");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DownloadRsaPrivateKey(string privateKeyPem)
        {
            if (string.IsNullOrWhiteSpace(privateKeyPem))
            {
                TempData["ErrorMessage"] = "No private key available for download.";
                return RedirectToAction(nameof(Index));
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(privateKeyPem);
            return File(bytes, "application/x-pem-file", "rsa-private-key.pem");
        }
    }
}