using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    public class AesCrypto
    {
        //----------------------------------------------------------------------
        // 暗号化する
        //----------------------------------------------------------------------
        public static string Encrypt(string input, string key, string iv)
        {
            var keyBytes = hexStringToByteArray(key);
            var ivBytes = hexStringToByteArray(iv);

            var rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = ivBytes
            };
            input = input.Replace("\r\n", "\n");
            var plainBytes = Encoding.UTF8.GetBytes(input);
            var bytes = rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            var base64Text = Convert.ToBase64String(bytes);
            
            return InsertLineBreaks(base64Text);
        }

        public static String InsertLineBreaks(String s)
        {
            int len = s.Length;
            var data = new StringBuilder(len + len / 64);

            for (int i = 0; i < len; i += 64)
            {
                if (i + 64 < len)
                {
                    data.Append(s.Substring(i, 64)); data.Append("\n");
                }
                else
                {
                    data.Append(s.Substring(i));
                }
            }

            return data.ToString();
        }

        //----------------------------------------------------------------------
        // 復号する
        //----------------------------------------------------------------------
        public static string Decrypt(string input, string key, string iv)
        {
            var keyBytes = hexStringToByteArray(key);
            var ivBytes = hexStringToByteArray(iv);

            var rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = ivBytes
            };
            byte[] cipheredBytes = Convert.FromBase64String(input);
            var bytes = rijndaelManaged.CreateDecryptor().TransformFinalBlock(cipheredBytes, 0, cipheredBytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        
        public static byte[] hexStringToByteArray(String s)
        {
            int len = s.Length;
            byte[] data = new byte[len / 2];
            for (int i = 0; i < len; i += 2)
            {
                data[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }
            return data;
        }
    }
}
