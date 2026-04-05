using System.Security.Cryptography;
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AesEncryptionService : IAesEncryptionService
    {
        public AesEncryptionResult Encrypt(string plaintext, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = ConfigureAes(key, iv, cipherMode, paddingMode);
            
            using ICryptoTransform encryptor = aes.CreateEncryptor();

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);
            streamWriter.Write(plaintext);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            AesEncryptionResult result = new()
            {
                EncryptedText = Convert.ToBase64String(memoryStream.ToArray())
            };

            return result;
        }

        public AesDecryptionResult Decrypt(string ciphertext, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = ConfigureAes(key, iv, cipherMode, paddingMode);
            
            using ICryptoTransform decryptor = aes.CreateDecryptor();

            using MemoryStream memoryStream = new(Convert.FromBase64String(ciphertext));
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            AesDecryptionResult result = new()
            {
                DecryptedText = streamReader.ReadToEnd()
            };
            
            return result;
        }

        public AesEncryptionResult EncryptFile(FileInfo file, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = ConfigureAes(key, iv, cipherMode, paddingMode);
            
            using ICryptoTransform encryptor = aes.CreateEncryptor();

            using FileStream inFs = new(file.FullName, FileMode.Open);
            using CryptoStream cryptoStream = new(inFs, encryptor, CryptoStreamMode.Read);
            using FileStream outFs = new(Path.Combine(file.DirectoryName, $"{file.Name}.encrypted"), FileMode.Create);
            
            cryptoStream.CopyTo(outFs);
            var result = new AesEncryptionResult
            {
                FileInfo = new FileInfo(outFs.Name)
            };

            return result;
        }

        public AesDecryptionResult DecryptFile(FileInfo file, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = ConfigureAes(key, iv, cipherMode, paddingMode);
            
            using ICryptoTransform decryptor = aes.CreateDecryptor();

            using FileStream inFs = new(file.FullName, FileMode.Open);
            using CryptoStream cryptoStream = new(inFs, decryptor, CryptoStreamMode.Read);
            using FileStream outFs = new(Path.Combine(file.DirectoryName, $"{file.Name}.decrypted"), FileMode.Create);

            cryptoStream.CopyTo(outFs);
            var result = new AesDecryptionResult
            {
                FileInfo = new FileInfo(outFs.Name)
            };

            return result;
        }

        private Aes ConfigureAes(string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            return aes;
        }
    }
}
