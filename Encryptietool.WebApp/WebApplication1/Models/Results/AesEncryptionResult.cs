namespace WebApplication1.Models.Results
{
    public class AesEncryptionResult : BaseResult
    {
        public string CipherText { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}