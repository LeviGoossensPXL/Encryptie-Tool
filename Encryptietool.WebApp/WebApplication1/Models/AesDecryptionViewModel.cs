namespace WebApplication1.Models
{
    public class AesDecryptionViewModel
    {
        public string? InputText { get; set; }
        public IFormFile? InputFile { get; set; }
        
        public bool IsFileUpload => InputFile != null;

        public string Key { get; set; }
        public string IV { get; set; }

        public string CipherMode { get; set; }
        public string PaddingMode { get; set; }
        
        public string? OutputText { get; set; }
        public string? OutputFile { get; set; }
    }
}
