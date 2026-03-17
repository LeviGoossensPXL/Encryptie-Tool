namespace WebApplication1.Models.Results
{
    public class AesEncryptionResult
    {
        public byte[] Ciphertext { get; set; }
        public string metadata { get; set; }
    }
}