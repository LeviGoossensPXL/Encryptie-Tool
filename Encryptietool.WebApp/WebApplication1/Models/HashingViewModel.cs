using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class HashingViewModel
    {
        public string? InputText { get; set; }
        public IFormFile? InputFile { get; set; }
        public bool IsFileUpload { get; set; }

        [Required]
        public HashAlgorithmType HashAlgorithm { get; set; } = HashAlgorithmType.SHA256;

        public string? GeneratedHash { get; set; }
        public string? ExpectedHash { get; set; }
        public bool? VerificationSucceeded { get; set; }

        public string? HmacInputText { get; set; }
        public IFormFile? HmacInputFile { get; set; }
        public bool IsHmacFileUpload { get; set; }

        [Required]
        public HmacAlgorithmType HmacAlgorithm { get; set; } = HmacAlgorithmType.HMACSHA256;

        public string? HmacSecretKey { get; set; }
        public string? GeneratedHmac { get; set; }
        public string? ExpectedHmac { get; set; }
        public bool? HmacVerificationSucceeded { get; set; }

        public string? WarningMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
