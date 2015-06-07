using System;
using System.Security.Cryptography;
using System.Text;
using FinanceMonitor.Config;

namespace FinanceMonitor.Data
{
    public class Encryption
    {
        private static string key = AppConfiguration.Current.EncryptionKey;

        public static string CreateDBKey()
        {
            return new Random().Next(int.MaxValue).ToString();
        }

        public static string DecryptDBKey(string text)
        {
            return Decrypt(text);
        }

        public static string Encrypt(string text)
        {
            return EncryptValue(text, key);
        }

        public static string Encrypt(string text, string dbKey)
        {
            return EncryptValue(text, key + dbKey);
        }

        private static string EncryptValue(string text, string fullKey)
        {
            byte[] buff = ASCIIEncoding.ASCII.GetBytes(text);

            return Convert.ToBase64String((new TripleDESCryptoServiceProvider()
            {
                Key = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(fullKey)),
                Mode = CipherMode.ECB
            }).CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length)).Replace("+", "!");            
        }

        public static string Decrypt(string text)
        {
            return DecryptValue(text, key);
        }

        public static string Decrypt(string text, string dbKey)
        {
            return DecryptValue(text, key + dbKey);
        }

        private static string DecryptValue(string text, string fullKey)
        {
            byte[] buf = Convert.FromBase64String(text.Replace("!", "+"));

            return ASCIIEncoding.ASCII.GetString((new TripleDESCryptoServiceProvider()
            {
                Key = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(fullKey)),
                Mode = CipherMode.ECB
            }).CreateDecryptor().TransformFinalBlock(buf, 0, buf.Length));
        }
    }
}
