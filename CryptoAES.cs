using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
namespace Framework.Security
{
    public class CryptoAES
    {
        public static int[] aesKeySize = { 128, 192, 256 };
        public static int aesIVSize = 128;
        ICryptoTransform encrypter;
        ICryptoTransform decrypter;
        public void Initialize(string plainKeyString, string playIVString)
        {
            var aesKeyBase64 = Crypto.EncodingBase64(plainKeyString);
            var aesIVBase64 = Crypto.EncodingBase64(playIVString);

            var key = Convert.FromBase64String(aesKeyBase64);
            var iv = Convert.FromBase64String(aesIVBase64);

            var rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.KeySize = key.Length * 8;
            rijndaelManaged.BlockSize = aesIVSize;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;

            encrypter = rijndaelManaged.CreateEncryptor();
            decrypter = rijndaelManaged.CreateDecryptor();

        }
        public string Encrypt(string plainText)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypter, CryptoStreamMode.Write))
                {
                    var byteData = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(byteData, 0, byteData.Length);
                }
                byte[] byteCrypto = memoryStream.ToArray();
                return Convert.ToBase64String(byteCrypto);
            }
        }
        public byte[] Encrypt(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypter, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }
                return memoryStream.ToArray();
            }
        }
        public string Decrypt(string encryptData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Write))
                {
                    var byteEncrypt = Convert.FromBase64String(encryptData);
                    cryptoStream.Write(byteEncrypt, 0, byteEncrypt.Length);
                }
                byte[] byteCrypto = memoryStream.ToArray();
                return Encoding.UTF8.GetString(byteCrypto);
            }
        }
        public byte[] Decrypt(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Write))
                {
                    try
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Decrypt bytes " + e + "\n data length : " + data.Length);
                    }


                }
                return memoryStream.ToArray();
            }
        }
        public byte[] DecryptTest(string encryptData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Write))
                {
                    var byteEncrypt = Convert.FromBase64String(encryptData);
                    cryptoStream.Write(byteEncrypt, 0, byteEncrypt.Length);
                }
                return memoryStream.ToArray();
            }
        }

    }

}
