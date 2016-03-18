using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using IndDev.Auth.Model;

namespace IndDev.Auth.Logic
{
    public class SecureLogic
    {
        
        public static string EncodeMd5(string s)
        {
            var md5 = MD5.Create();
            var inputButes = Encoding.UTF8.GetBytes(s);
            var hash = md5.ComputeHash(inputButes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        
    }
    public class ValidationInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}