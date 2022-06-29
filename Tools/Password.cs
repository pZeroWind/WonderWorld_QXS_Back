using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Password
    {
        private const string _key = "KARFB_HKDKD_WOF_WGD_GEB_GROUNDZIO";

        /// <summary>
        /// 隐藏参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHide(this string value)
        {
            string result = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                if (i < 5&&i > value.Length - 4)
                {
                    result += "*";
                }
                else
                {
                    result += value[i];
                }
                
            }
            return result;
        }
        /// <summary>
        /// 将密码进行加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ToSHA256Encrypt(this string password)
        {
            return password.ToSHA256().Encrypt();
        }

        public static string ToSHA256(this string password)
        {
            //使用SHA256加密
            SHA256 sha256 = SHA256.Create();
            byte[] resBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte c in resBytes)
            {
                builder.Append(c.ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取盐随机数
        /// </summary>
        /// <returns></returns>
        public static string GetSalt()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            return random.Next(1000,10000).ToString();
        }

        /// <summary>
        /// RAS加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(this string text)
        {
            try
            {
                CspParameters param = new CspParameters();
                param.KeyContainerName = _key;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    byte[] encryptData = rsa.Encrypt(data, false);
                    return Convert.ToHexString(encryptData);
                }
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// RAS解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(this string text)
        {
            try
            {
                CspParameters param = new CspParameters();
                param.KeyContainerName = _key;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
                {
                    byte[] data = Convert.FromHexString(text.Trim());
                    byte[] encryptData = rsa.Decrypt(data, false);
                    return Encoding.UTF8.GetString(encryptData);
                }
            }
            catch
            {
                return "error";
            }
        }

    }
}
