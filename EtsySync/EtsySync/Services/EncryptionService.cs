using EtsySync.Data;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;
using System.Security.Cryptography;


namespace EtsySync.Services
{
        public class EncryptionService
        {
            private readonly InvoiceDbContext _context;
            private readonly string _keyStorageTable = "EncryptionKeys";
            private readonly string _defaultEncryptionKey = "your-fallback-key";

            public EncryptionService(InvoiceDbContext context)
            {
                _context = context;
            }


            public async Task<string> GenerateAndStoreEncryptionKeyAsync()
            {
                var newKey = GenerateSecureKey();
                var encryptionKey = new EncryptionKey
                {
                    Key = newKey,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.EncryptionKeys.AddAsync(encryptionKey);
                await _context.SaveChangesAsync();

                return newKey;
            }


            public async Task<string> GetEncryptionKeyAsync()
            {
                var encryptionKey = await _context.EncryptionKeys.OrderByDescending(k => k.CreatedAt).FirstOrDefaultAsync();

                if (encryptionKey != null)
                {
                    return encryptionKey.Key;
                }


                return await GenerateAndStoreEncryptionKeyAsync();
            }

            private string GenerateSecureKey()
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] keyBytes = new byte[32];
                    rng.GetBytes(keyBytes);
                    return Convert.ToBase64String(keyBytes);
                }
            }


            public byte[] EncryptData(byte[] data, string encryptionKey)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Convert.FromBase64String(encryptionKey);
                    aesAlg.IV = new byte[16];

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }


            public byte[] DecryptData(byte[] encryptedData, string encryptionKey)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Convert.FromBase64String(encryptionKey);
                    aesAlg.IV = new byte[16];

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (MemoryStream msResult = new MemoryStream())
                    {
                        csDecrypt.CopyTo(msResult);
                        return msResult.ToArray();
                    }
                }
            }
        }
}

