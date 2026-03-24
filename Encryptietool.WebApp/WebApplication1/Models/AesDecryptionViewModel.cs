using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AesDecryptionViewModel
    {
        public string? InputText { get; set; }
        public IFormFile? InputFile { get; set; }
        
        public bool IsFileUpload => InputFile != null;
        [Required]
        public string Key { get; set; }
        [Required]
        public string IV { get; set; }

        [Required]
        public string CipherMode { get; set; }
        [Required]
        public string PaddingMode { get; set; }
        
        public string? OutputText { get; set; }
        public string? OutputFile { get; set; }
    }
}
