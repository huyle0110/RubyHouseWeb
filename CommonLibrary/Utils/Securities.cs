using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Utils
{
    public class Securities
    {
        public static string SHA1(string data)
        {
            return BitConverter.ToString(encryptData(string.Join("", data, "RubyHouseWeb"))).Replace("-", "").ToLower();
        }

        public static string md5(string data)
        {
            return BitConverter.ToString(encryptDataMD5(data)).Replace("-", "").ToLower();
        }
        static byte[] encryptDataMD5(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        static byte[] encryptData(string data)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider SHA1Hasher = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = SHA1Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
    }
}
