using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class KeyGenerationViewModel
    {
        [Required]
        public string KeyType { get; set; } = "AES";

        [Required]
        public int KeySize { get; set; } = 256;

        public AesKeyResult? AesResult { get; set; }
        public RsaKeyResult? RsaResult { get; set; }

        public string? ErrorMessage { get; set; }
    }
}