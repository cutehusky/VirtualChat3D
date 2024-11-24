using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Core.MVC;
using RSA = DevsDaddy.Shared.CryptoLibrary.Modules.RSA;

namespace Core.NetworkModule.Model
{
    public class EncryptionProvider: ModelBase
    {
        #region AES
        private readonly byte[] _aesKey = new byte[16];
        public byte[] AESEncrytedKey => _client2ServerEncryption.EncryptBinary(_aesKey);
        
        private void CreateAESKey()
        {
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(_aesKey);
        }

        public byte[] EncryptAES128ECB(string plaintext)
        {
            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public string DecryptAES128ECB(byte[] ciphertext)
        {
            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        #endregion

        #region RSA
        private RSA.RSAProvider _client2ServerEncryption;

        public void SetServerKey(string key)
        {
            _client2ServerEncryption = new RSA.RSAProvider(new RSA.RSAEncryptionOptions() {
                publicKey = key,
            });
        }
        #endregion

        
        protected override void OnInit()
        {
            CreateAESKey();
        }
    }
}
