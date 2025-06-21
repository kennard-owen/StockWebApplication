﻿using System.Security.Cryptography;
using System.Text;
using PF.Utils.Check;
using PF.Utils.Extensions;

namespace PF.Utils.Security
{
    public static class SecurityUtil
    {
        public static string ToMd5(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                //for (var i = 0; i < data.Length; i++)
                //{
                //    sBuilder.Append(data[i].ToString("x2"));
                //}
                return data.ToHexString();
            }
        }

        public static string ToMd52(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            return data.ToHexString();
        }

        public static string ToSha512(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            SHA512 shaM = new SHA512Managed();
            var result = shaM.ComputeHash(str.ToBytes());
            return result.ToHexString();
        }

        public static string ToSha1(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            SHA1 shaM = new SHA1Managed();
            var result = shaM.ComputeHash(str.ToBytes());
            return result.ToHexString();
        }

        public static string ToHexString(this byte[] bytes)
        {
            var sBuilder = new StringBuilder();
            foreach (var t in bytes)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}