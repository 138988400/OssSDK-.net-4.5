using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Utilities
{
    internal static class HttpUtils
    {
        public const string Charset = "utf-8";
        public const string Iso88591Charset = "iso-8859-1";

        public static string GetRequestParameterString(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isFirst = true;
            foreach (KeyValuePair<string, string> p in parameters)
            {
                if (!isFirst)
                {
                    stringBuilder.Append("&");
                }
                isFirst = false;
                stringBuilder.Append(p.Key);
                if (p.Value != null)
                {
                    stringBuilder.Append("=").Append(UrlEncode(p.Value, "utf-8"));
                }
            }
            return stringBuilder.ToString();
        }

        public static string ReEncode(string text, string fromCharset, string toCharset)
        {
            byte[] buffer = Encoding.GetEncoding(fromCharset).GetBytes(text);
            return Encoding.GetEncoding(toCharset).GetString(buffer);
        }

        public static string UrlEncode(string data, string charset)
        {
            StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            foreach (char c in Encoding.GetEncoding(charset).GetBytes(data))
            {
                if (text.IndexOf(c) != -1)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("%").Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", new object[] { (int)c }));
                }
            }
            return stringBuilder.ToString();
        }
    }
}
