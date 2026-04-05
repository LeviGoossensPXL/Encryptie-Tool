using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class HashingController : Controller
    {
        private readonly IHashingService _hashingService;
        private readonly ILogger<HashingController> _logger;

        public HashingController(IHashingService hashingService, ILogger<HashingController> logger)
        {
            _hashingService = hashingService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HashingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateHash(HashingViewModel model)
        {
            try
            {
                SetHashWarning(model);

                if (model.IsFileUpload)
                {
                    if (model.InputFile == null || model.InputFile.Length == 0)
                    {
                        model.ErrorMessage = "Selecteer eerst een bestand.";
                        return View("Index", model);
                    }

                    using var stream = model.InputFile.OpenReadStream();
                    model.GeneratedHash = _hashingService.ComputeFileHash(stream, model.HashAlgorithm);
                }
                else
                {
                    model.GeneratedHash = _hashingService.ComputeHash(model.InputText ?? string.Empty, model.HashAlgorithm);
                }

                ModelState.Remove(nameof(HashingViewModel.GeneratedHash));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating hash.");
                model.ErrorMessage = "Er ging iets mis bij het genereren van de hash.";
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyHash(HashingViewModel model)
        {
            try
            {
                SetHashWarning(model);

                if (string.IsNullOrWhiteSpace(model.ExpectedHash))
                {
                    model.ErrorMessage = "Voer een verwachte hash in.";
                    return View("Index", model);
                }

                if (model.IsFileUpload)
                {
                    if (model.InputFile == null || model.InputFile.Length == 0)
                    {
                        model.ErrorMessage = "Selecteer eerst een bestand.";
                        return View("Index", model);
                    }

                    using var stream = model.InputFile.OpenReadStream();
                    model.VerificationSucceeded = _hashingService.VerifyFileHash(
                        stream,
                        model.ExpectedHash,
                        model.HashAlgorithm);
                }
                else
                {
                    model.VerificationSucceeded = _hashingService.VerifyHash(
                        model.InputText ?? string.Empty,
                        model.ExpectedHash,
                        model.HashAlgorithm);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while verifying hash.");
                model.ErrorMessage = "Er ging iets mis bij het verifiëren van de hash.";
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateHmac(HashingViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.HmacSecretKey))
                {
                    model.ErrorMessage = "Voer een secret key in voor HMAC.";
                    return View("Index", model);
                }

                if (model.IsHmacFileUpload)
                {
                    if (model.HmacInputFile == null || model.HmacInputFile.Length == 0)
                    {
                        model.ErrorMessage = "Selecteer eerst een bestand voor HMAC.";
                        return View("Index", model);
                    }

                    using var stream = model.HmacInputFile.OpenReadStream();
                    model.GeneratedHmac = _hashingService.ComputeFileHmac(
                        stream,
                        model.HmacSecretKey,
                        model.HmacAlgorithm);
                }
                else
                {
                    model.GeneratedHmac = _hashingService.ComputeHmac(
                        model.HmacInputText ?? string.Empty,
                        model.HmacSecretKey,
                        model.HmacAlgorithm);
                }

                ModelState.Remove(nameof(HashingViewModel.GeneratedHmac));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating HMAC.");
                model.ErrorMessage = "Er ging iets mis bij het genereren van de HMAC.";
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyHmac(HashingViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.HmacSecretKey))
                {
                    model.ErrorMessage = "Voer een secret key in voor HMAC.";
                    return View("Index", model);
                }

                if (string.IsNullOrWhiteSpace(model.ExpectedHmac))
                {
                    model.ErrorMessage = "Voer een verwachte HMAC in.";
                    return View("Index", model);
                }

                if (model.IsHmacFileUpload)
                {
                    if (model.HmacInputFile == null || model.HmacInputFile.Length == 0)
                    {
                        model.ErrorMessage = "Selecteer eerst een bestand voor HMAC.";
                        return View("Index", model);
                    }

                    using var stream = model.HmacInputFile.OpenReadStream();
                    model.HmacVerificationSucceeded = _hashingService.VerifyFileHmac(
                        stream,
                        model.ExpectedHmac,
                        model.HmacSecretKey,
                        model.HmacAlgorithm);
                }
                else
                {
                    model.HmacVerificationSucceeded = _hashingService.VerifyHmac(
                        model.HmacInputText ?? string.Empty,
                        model.ExpectedHmac,
                        model.HmacSecretKey,
                        model.HmacAlgorithm);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while verifying HMAC.");
                model.ErrorMessage = "Er ging iets mis bij het verifiëren van de HMAC.";
            }

            return View("Index", model);
        }

        private static void SetHashWarning(HashingViewModel model)
        {
            model.WarningMessage = model.HashAlgorithm switch
            {
                HashAlgorithmType.MD5 => "MD5 is deprecated - use for learning only.",
                HashAlgorithmType.SHA1 => "SHA1 is considered weak and should not be used for security-critical purposes.",
                _ => null
            };
        }
    }
}
