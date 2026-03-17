namespace WebApplication1.Models
{
    public class EncryptionViewModel
    {
        public string InputText { get; set; }

        public string Key { get; set; }
        public string IV { get; set; }

        public string CipherMode { get; set; }
        public string PaddingMode { get; set; }
    }
}
