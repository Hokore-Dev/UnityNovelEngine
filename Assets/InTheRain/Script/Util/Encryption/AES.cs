using System.Security.Cryptography;
using System.Text;
using System;

namespace VNFramework
{
    public class AES
    {
        private static string _AESKey = string.Empty;

        public static void Initalize(string AESKey)
        {
            _AESKey = AESKey;

#if UNITY_EDITOR
            byte[] stream = UTF8Encoding.UTF8.GetBytes(_AESKey);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stream.Length; ++i)
            {
                sb.Append(stream[i].ToString("X"));
            }
#endif // UNITY_EDITOR
        }

        /// <summary>
        /// 암호화키를 32자리로 만들어 주는 함수.
        /// </summary>
        /// <param name="inRawSecretkey">사용자가 입력한 암호화 키.</param>
        /// <returns></returns>
        private static string SetSecretKey32(string inRawSecretkey)
        {
            if (inRawSecretkey.Length < 32)
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.Append(inRawSecretkey);

                int index = 0;
                while (s.Length < 32)
                {
                    s.Append(inRawSecretkey[index++ % inRawSecretkey.Length]);
                }

                return s.ToString();
            }
            else if (inRawSecretkey.Length > 32)
            {
                return inRawSecretkey.Substring(0, 32);
            }

            return inRawSecretkey;
        }

        /// <summary>
        /// 주의: 암호화를 위해서는 void Initalize(string AESKey)를 통한 AESKey 설정이 필요
        /// </summary>
        public static string Encrypt(string toEncrypt)
        {
            return Encrypt(toEncrypt, _AESKey);
        }

        public static string Encrypt(string toEncrypt, string key)
        {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(SetSecretKey32(key));
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 주의: 복호화를 위해서는 void Initalize(string AESKey)를 통한 AESKey 설정이 필요
        /// </summary>
        public static string Decrypt(string toDecrypt)
        {
            return Decrypt(toDecrypt, _AESKey);
        }

        public static string Decrypt(string toDecrypt, string key)
        {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(SetSecretKey32(key));
            byte[] toEncryptArray;

            try
            {
                toEncryptArray = System.Convert.FromBase64String(toDecrypt);
            }
            catch (Exception)
            {
                // 암호화되지 않은 문자열이므로 그대로 반환 
                return toDecrypt;
            }

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static byte[] EncryptFromStream(byte[] inByteArr)
        {
            if (_AESKey.Equals(string.Empty))
            {
                return null;
            }

            return EncryptFromStream(inByteArr, _AESKey);
        }

        public static byte[] EncryptFromStream(byte[] inData, string inKey)
        {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(SetSecretKey32(inKey));
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            return cTransform.TransformFinalBlock(inData, 0, inData.Length);
        }

        public static byte[] Decrypt(byte[] inByteArr)
        {
            if (_AESKey.Equals(string.Empty))
            {
                return null;
            }

            return DecryptFromStream(inByteArr, _AESKey);
        }

        public static byte[] DecryptFromStream(byte[] inData, string inKey)
        {
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes(SetSecretKey32(inKey));
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            return cTransform.TransformFinalBlock(inData, 0, inData.Length);
        }
    }
}
