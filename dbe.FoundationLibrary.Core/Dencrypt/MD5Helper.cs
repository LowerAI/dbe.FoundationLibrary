using System;
using System.Security.Cryptography;
using System.Text;

namespace dbe.FoundationLibrary.Core.Dencrypt
{
    //调用方式
    //public static void md5()
    //{
    //    var str1 = MD5Helper.Encrypt16Bit("123456789");
    //    var str2 = MD5Helper.Encrypt32Bit("123456789");
    //    var str3 = MD5Helper.Encrypt64Bit("123456789");
    //}

    /// <summary>
    /// MD5加解密
    /// </summary>
    public static class MD5Helper
    {
        /// <summary>
        /// MD5 16位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public static string Encrypt16Bit(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password)), 4, 8);
            //str = str.Replace("-", "");
            return str;
        }

        /// <summary>
        ///  MD5 32位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public static string Encrypt32Bit(string password)
        {
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < bytes.Length; i++)         // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                pwd += bytes[i].ToString("X").PadLeft(2, '0');        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            return pwd;
        }

        /// <summary>
        ///  MD5 64位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public static string Encrypt64Bit(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] str = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(str);
        }
    }
}