using System.Security.Cryptography;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AesEncryptionService : IAesEncryptionService
    {
        public EncryptionResult Encrypt(string plaintext, byte[] key, byte[] iv, CipherMode mode)
        {
            EncryptionResult result;
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            using ICryptoTransform encryptor = aes.CreateEncryptor();

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);
            streamWriter.Write(plaintext);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            result = new()
            {
                Ciphertext = memoryStream.ToArray()
            };

            return result;
        }

        public string Decrypt(byte[] ciphertext, byte[] key, byte[] iv, CipherMode mode)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            using ICryptoTransform decryptor = aes.CreateDecryptor();

            using MemoryStream memoryStream = new(ciphertext);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public EncryptionResult EncryptFile(string file, byte[] key, byte[] iv, CipherMode mode)
        {
            EncryptionResult result;
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using FileStream fsw = new FileStream(file, FileMode.Open, FileAccess.Read);
            using CryptoStream cryptoStream = new(fsw, encryptor, CryptoStreamMode.Read);
            using StreamWriter streamWriter = new(cryptoStream);
            throw new NotImplementedException();
        }

        public FileInfo DecryptFile(string encryptedFile, byte[] key, byte[] iv, CipherMode mode)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            using ICryptoTransform decryptor = aes.CreateDecryptor();

            using FileStream fsw = new FileStream(encryptedFile, FileMode.Open, FileAccess.Read);
            using CryptoStream cryptoStream = new(fsw, decryptor, CryptoStreamMode.Read);
            using StreamWriter streamReader = new(cryptoStream);
            throw new NotImplementedException();
        }
    }
}
