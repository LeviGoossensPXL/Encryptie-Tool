using System.Security.Cryptography;
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AesEncryptionService : IAesEncryptionService
    {
        public AesEncryptionResult Encrypt(string plaintext, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            using ICryptoTransform encryptor = aes.CreateEncryptor();

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);
            streamWriter.Write(plaintext);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            AesEncryptionResult result = new()
            {
                Ciphertext = memoryStream.ToArray()
            };

            return result;
        }

        public AesDecryptionResult Decrypt(byte[] ciphertext, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            using ICryptoTransform decryptor = aes.CreateDecryptor();

            using MemoryStream memoryStream = new(ciphertext);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            AesDecryptionResult result = new()
            {
                DecryptedText = streamReader.ReadToEnd()
            };
            return result;
        }

        public FileInfo EncryptFile(FileInfo file, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            AesEncryptionResult result;
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            using ICryptoTransform encryptor = aes.CreateEncryptor();


            using FileStream inFs = new(file.FullName, FileMode.Open);

            using CryptoStream cryptoStream = new(inFs, encryptor, CryptoStreamMode.Read);

            using FileStream outFs = new(Path.Combine(file.DirectoryName, "tmp.enc"), FileMode.Create);

            byte[] buffer = new byte[2048];
            while (cryptoStream.Read(buffer, 0, buffer.Length) > 0)
            {
                outFs.Write(buffer, 0, buffer.Length);
            }

            inFs.Close();
            outFs.Close();

            return new FileInfo(outFs.Name);
        }

        public FileInfo DecryptFile(FileInfo encryptedFile, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            AesEncryptionResult result;
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            using ICryptoTransform decryptor = aes.CreateDecryptor();


            using FileStream inFs = new(encryptedFile.FullName, FileMode.Open);

            using CryptoStream cryptoStream = new(inFs, decryptor, CryptoStreamMode.Read);

            using FileStream outFs = new(Path.Combine(encryptedFile.DirectoryName, "tmp.dec"), FileMode.Create);

            byte[] buffer = new byte[2048];
            while (cryptoStream.Read(buffer, 0, buffer.Length) > 0)
            {
                outFs.Write(buffer, 0, buffer.Length);
            }

            inFs.Close();
            outFs.Close();

            return new FileInfo(outFs.Name);
        }
    }
}
