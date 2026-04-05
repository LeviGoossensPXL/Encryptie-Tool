namespace WebApplication1.Models.Results
{
    public class AesDecryptionResult : BaseResult
    {
        public string PlainText { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}