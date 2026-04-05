namespace WebApplication1.Models.Results
{
    public class AesKeyResult
    {
        public byte[] KeyBytes { get; set; } = Array.Empty<byte>();
        public byte[] IVBytes { get; set; } = Array.Empty<byte>();

        public string KeyBase64 { get; set; } = string.Empty;
        public string KeyHex { get; set; } = string.Empty;

        public string IVBase64 { get; set; } = string.Empty;
        public string IVHex { get; set; } = string.Empty;

        public int KeySize { get; set; }
    }
}