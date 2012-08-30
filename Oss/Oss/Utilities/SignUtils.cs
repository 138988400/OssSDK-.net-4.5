using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Utilities
{
   internal class SignUtils
    {
        private const string _newLineMarker = "\n";
        private static IList<string> SIGNED_PARAMTERS = new List<string> { "acl", "uploadId", "partNumber", "uploads", "response-cache-control", "response-content-disposition", "response-content-encoding", "response-content-language", "response-content-type", "response-expires" };

        private static string BuildCanonicalizedResource(OssHttpRequestMessage httpRequestMessage)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(httpRequestMessage.ResourcePath);
            if (httpRequestMessage.Parameters != null)
            {
                IOrderedEnumerable<string> parameterNames = from e in httpRequestMessage.Parameters.Keys
                    orderby e
                    select e;
                char separater = '?';
                foreach (string paramName in parameterNames)
                {
                    if (SIGNED_PARAMTERS.Contains(paramName))
                    {
                        builder.Append(separater);
                        builder.Append(paramName);
                        string paramValue = httpRequestMessage.Parameters[paramName];
                        if (paramValue != null)
                        {
                            builder.Append("=").Append(paramValue);
                        }
                        separater = '&';
                    }
                }
            }
            return builder.ToString();
        }

        public static string BuildCanonicalString(OssHttpRequestMessage httpRequestMessage)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.Append(httpRequestMessage.Method).Append("\n");
            IDictionary<string, string> headersToSign = new Dictionary<string, string>();

            if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.ContentType != null)
            {
                headersToSign.Add("Content-Type".ToLowerInvariant(), httpRequestMessage.Content.Headers.ContentType.MediaType);
            }
            if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.ContentMD5 != null)
            {
                headersToSign.Add("Content-MD5".ToLowerInvariant(), httpRequestMessage.Content.Headers.ContentMD5.ToString());
            }

            headersToSign.Add("Date".ToLowerInvariant(), DateUtils.FormatRfc822Date(httpRequestMessage.Headers.Date.Value.UtcDateTime));

            if (!headersToSign.ContainsKey("Content-Type".ToLowerInvariant()))
            {
                headersToSign.Add("Content-Type".ToLowerInvariant(), "");
            }

            if (!headersToSign.ContainsKey("Content-MD5".ToLowerInvariant()))
            {
                headersToSign.Add("Content-MD5".ToLowerInvariant(), "");
            }

            if (httpRequestMessage.Parameters != null)
            {
                foreach (KeyValuePair<string, string> p in httpRequestMessage.Parameters)
                {
                    if (p.Key.StartsWith("x-oss-"))
                    {
                        headersToSign.Add(p.Key, p.Value);
                    }
                }
            }

            foreach (KeyValuePair<string, string> entry in from e in headersToSign
                                                           orderby e.Key
                                                           select e)
            {
                string key = entry.Key;
                object value = entry.Value;
                if (key.StartsWith("x-oss-"))
                {
                    builder.Append(key).Append(':').Append(value);
                }
                else
                {
                    builder.Append(value);
                }
                builder.Append("\n");
            }

            builder.Append(BuildCanonicalizedResource(httpRequestMessage));
            return builder.ToString();
        }
    }
}
