using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Security
{
    public class MD5
    {
        public static string MD5Encrypt(string strText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            return BitConverter.ToString(result).ToLower().Replace("-", "");
        }
    }
}
