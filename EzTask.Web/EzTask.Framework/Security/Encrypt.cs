using System;
using System.Security.Cryptography;
using System.Text;

namespace EzTask.Framework.Security
{
    public class Encrypt
    {
        public static string Do(string data, string key)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(key));


            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };


            var dataToEncrypt = utf8.GetBytes(data);

            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }
    }
}
