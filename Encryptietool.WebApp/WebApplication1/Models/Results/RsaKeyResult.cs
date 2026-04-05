namespace WebApplication1.Models
{
    public class RsaKeyResult
    {
        public string PublicKeyPem { get; set; } = string.Empty;
        public string PrivateKeyPem { get; set; } = string.Empty;
        public int KeySize { get; set; }
    }
}