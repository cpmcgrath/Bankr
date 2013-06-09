using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CMcG.Bankr.Logic
{
    public class TwoWayEncryption
    {
        public TwoWayEncryption(string password = DEFAULT_PASSWORD)
        {
            KeyBytes = new Rfc2898DeriveBytes(password, MakeSalt(), 10000);
        }

        public Rfc2898DeriveBytes KeyBytes { get; private set; }

        public string Encrypt(string dataToEncrypt)
        {
            using (var aes          = new AesManaged { Key = KeyBytes.GetBytes(32), IV = KeyBytes.GetBytes(16) })
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string Decrypt(string dataToDecrypt)
        {
            using (var aes          = new AesManaged { Key = KeyBytes.GetBytes(32), IV = KeyBytes.GetBytes(16) })
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                byte[] data = Convert.FromBase64String(dataToDecrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                byte[] decryptBytes = memoryStream.ToArray();
                return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }
        }

        byte[] MakeSalt()
        {
            return Encoding.UTF8.GetBytes("YtPjUgYZTZii6ILnr85tKMSOdMUgQciY7oLXyMnl5CKWxb1mGFLIV5UEw0Dq");
        }

        public const string DEFAULT_PASSWORD = "02Gzbti2HV9w8YMNRf"; //You may want to change this value
    }
}