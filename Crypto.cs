using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Framework.Security
{
    public class Crypto
    {
        private static Dictionary<string, CryptoAES> aesDic = new Dictionary<string, CryptoAES>();
        private static CryptoRSA rsa;
        private static string PREFIX = "DayBrixEncryption_";
        public static bool IsEncrypted(string text)
        {
            return text.Length > PREFIX.Length && text.Substring(0, PREFIX.Length).Equals(PREFIX);
        }
        public static bool IsEncrypted(byte[] data)
        {
            var encodingPrefix = Encoding.UTF8.GetBytes(PREFIX);
            if (data.Length < encodingPrefix.Length)
            {
                return false;
            }
            var extractPrefix = data[0..encodingPrefix.Length];
            if (extractPrefix.SequenceEqual(encodingPrefix))
            {
                return true;
            }
            return false;
        }
        #region AES
        public static string EncryptAES(string plainText, string key, string iv)
        {
            CreateCryptoAES(key, iv);
            return PREFIX + aesDic[key].Encrypt(plainText);
        }
        public static byte[] EncryptAES(byte[] data, string key, string iv)
        {
            CreateCryptoAES(key, iv);
            var encryptedData = aesDic[key].Encrypt(data);
            var encodingPrefix = Encoding.UTF8.GetBytes(PREFIX);
            var result = new byte[encodingPrefix.Length + encryptedData.Length];
            encodingPrefix.CopyTo(result, 0);
            encryptedData.CopyTo(result, encodingPrefix.Length);
            return result;
        }
        public static string DecryptAES(string encryptData, string key, string iv)
        {
            if (!IsEncrypted(encryptData))
            {
                return encryptData;
            }
            encryptData = encryptData.Substring(PREFIX.Length, encryptData.Length - PREFIX.Length);
            CreateCryptoAES(key, iv);
            return aesDic[key].Decrypt(encryptData);
        }
        public static byte[] DecryptAES(byte[] data, string key, string iv)
        {
            if (!IsEncrypted(data))
            {
                return data;
            }
            var encodingPrefix = Encoding.UTF8.GetBytes(PREFIX);
            data = data[encodingPrefix.Length..data.Length];
            CreateCryptoAES(key, iv);
            return aesDic[key].Decrypt(data);
        }
        #endregion
        #region RSA

        public static string EncryptRSA(string data)
        {
            CreateRSA();
            return PREFIX + rsa.Encrypt(data);
        }
        public static string DecryptRSA(string data)
        {
            CreateRSA();
            return rsa.Decrypt(data);
        }
        public static byte[] EncryptRSA(byte[] data)
        {
            CreateRSA();
            var encryptedData = rsa.Encrypt(data);
            var encodingPrefix = Encoding.UTF8.GetBytes(PREFIX);
            var result = new byte[encodingPrefix.Length + encryptedData.Length];
            encodingPrefix.CopyTo(result, 0);
            encryptedData.CopyTo(result, encodingPrefix.Length);
            return result;
        }
        public static byte[] DecryptRSA(byte[] data)
        {
            if (!IsEncrypted(data))
                return data;

            CreateRSA();

            var encodingPrefix = Encoding.UTF8.GetBytes(PREFIX);
            data = data[encodingPrefix.Length..data.Length];
            return rsa.Decrypt(data);
        }
        private static void CreateRSA()
        {
            if (rsa == null)
            {
                rsa = new CryptoRSA();
                rsa.Initialize();
            }
        }
        #endregion
        public static string EncodingBase64(string plainText)
        {
            var strByte = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(strByte);
        }
        public static string DecodingBase64(string base64PlainText)
        {
            var strByte = Convert.FromBase64String(base64PlainText);
            return Encoding.UTF8.GetString(strByte);
        }

        private static void CreateCryptoAES(string key, string iv)
        {
            if (!aesDic.ContainsKey(key))
            {
                var aes = new CryptoAES();
                aes.Initialize(key, iv);
                aesDic.Add(key, aes);
            }
        }
    }

}
