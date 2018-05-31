using System;
using System.Security.Cryptography;
using System.Text;

namespace EzTask.Framework.Security
{
    public class Decrypt
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


            var dataToDecrypt = Convert.FromBase64String(data);

            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }


            return utf8.GetString(results);
        }
    }
}
