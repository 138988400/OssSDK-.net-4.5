using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Oss.Utilities
{
    internal static class OssUtils
    {
        public const int BufferSize = 0x2000;
        public const string Charset = "utf-8";
        public static readonly Uri DefaultEndpoint = new Uri("http://oss.aliyuncs.com");
        public const long MaxFileSzie = 0x140000000L;

        public static bool IsBucketNameValid(string bucketName)
        {
            string pattern = @"^[a-z0-9][a-z0-9_\-]{2,254}$";
            Regex regex = new Regex(pattern);
            return regex.Match(bucketName).Success;
        }

        public static bool IsObjectKeyValid(string key)
        {
            int byteCount = Encoding.GetEncoding("utf-8").GetByteCount(key);
            return ((byteCount > 0) && (byteCount < 0x400));
        }

        public static string MakeResourcePath(string bucket, string key)
        {
            if (bucket != null)
            {
                return (bucket + ((key != null) ? ("/" + UrlEncodeKey(key)) : ""));
            }
            return null;
        }

        public static string TrimETag(string eTag)
        {
            if (eTag == null)
            {
                return null;
            }
            return eTag.Trim(new char[] { '"' });
        }

        private static string UrlEncodeKey(string key)
        {
            string[] keys = key.Split(new char[] { '/' });
            StringBuilder uri = new StringBuilder();
            uri.Append(HttpUtils.UrlEncode(keys[0], "utf-8"));
            for (int i = 1; i < keys.Length; i++)
            {
                uri.Append('/').Append(HttpUtils.UrlEncode(keys[i], "utf-8"));
            }
            char temp = '/';
            if (key.EndsWith(temp.ToString()))
            {
                foreach (char keyChar in key)
                {
                    if (keyChar != '/')
                    {
                        break;
                    }
                    uri.Append('/');
                }
            }
            return uri.ToString();
        }
    }
}
