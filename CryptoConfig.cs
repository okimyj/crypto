using MoonSharp.Interpreter;
using UnityEngine;

namespace Framework.Security
{
    [CreateAssetMenu(fileName = "CryptoConfig", menuName = "Scriptable Object/Crypto Config", order = int.MaxValue)]
    public class CryptoConfig : ScriptableObject
    {
        public int AESKeySize = 128;
        public string AESKey = "daybrixAES128!!!";
        public int AESIVSize = 128;
        public string AESIV = "daybrixAESIV128!";
        public string RSAPrivateKeyFileName = "RSA_privateKey.xml"; //$"{Application.persistentDataPath}/RSA_privateKey.xml";
        public string RSAPublicKey = "";
    }

}
