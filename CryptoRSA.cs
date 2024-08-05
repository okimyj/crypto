using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Framework.Security
{
    public class CryptoRSA
    {
        // private 키는 file로 저장해놓고 bundle 에는 포함 x.
        // public 키는 Config에 set.
        private readonly string PRIVATE_KEY_PATH = Application.persistentDataPath;
        private CryptoConfig cryptoConfig;
        private string privateKey;
        public void Initialize()
        {
            var cryptoConfig = Resources.Load<CryptoConfig>("CryptoConfig");

            LoadCryptoConfig();
        }


        public void CreateRSAKeyPair()
        {
            LoadCryptoConfig();
            // create private key and save file.
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            var privateKeyParam = RSA.Create().ExportParameters(true);
            rsaCryptoServiceProvider.ImportParameters(privateKeyParam);

            privateKey = rsaCryptoServiceProvider.ToXmlString(true);
            File.WriteAllText($"{cryptoConfig.RSAPrivateKeyFileName}", privateKey);

            // create public key and cryptoConfig set.
            var publicKeyParam = RSA.Create().ExportParameters(false);
            publicKeyParam.Modulus = privateKeyParam.Modulus;
            publicKeyParam.Exponent = privateKeyParam.Exponent;
            rsaCryptoServiceProvider.ImportParameters(publicKeyParam);
            var publicKey = rsaCryptoServiceProvider.ToXmlString(false);
            cryptoConfig.RSAPublicKey = publicKey;
            EditorUtility.SetDirty(cryptoConfig);
        }

        public string Encrypt(string data)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(cryptoConfig.RSAPublicKey);
            var encryptedData = rsaCryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(data), true);
            Debug.Log($"Encrypted Data : {encryptedData}");
            return Convert.ToBase64String(encryptedData);
        }
        public string Decrypt(string data)
        {
            CheckPrivateKey();
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(privateKey);
            var decryptedData = rsaCryptoServiceProvider.Decrypt(Encoding.UTF8.GetBytes(data), true);
            Debug.Log($"Decrypted Data : {decryptedData}");
            return Convert.ToBase64String(decryptedData);
        }
        public byte[] Encrypt(byte[] data)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(cryptoConfig.RSAPublicKey);
            var encryptedData = rsaCryptoServiceProvider.Encrypt(data, true);
            return encryptedData;
        }
        public byte[] Decrypt(byte[] data)
        {
            CheckPrivateKey();
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(privateKey);
            var decryptedData = rsaCryptoServiceProvider.Decrypt(data, true);
            return decryptedData;
        }

        private void LoadCryptoConfig()
        {
            if (cryptoConfig == null)
                cryptoConfig = Resources.Load<CryptoConfig>("CryptoConfig");
        }
        private void CheckPrivateKey()
        {
            if (!string.IsNullOrEmpty(privateKey))
                return;
            string data = File.ReadAllText($"{PRIVATE_KEY_PATH}/{cryptoConfig.RSAPrivateKeyFileName}");
            if (string.IsNullOrEmpty(data))
                CreateRSAKeyPair();
            privateKey = data;
        }

    }

}
