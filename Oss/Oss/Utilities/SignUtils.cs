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

        private static string BuildCanonicalizedResource(string resourcePath, IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(resourcePath);
            if (parameters != null)
            {
                IOrderedEnumerable<string> parameterNames = from e in parameters.Keys
                    orderby e
                    select e;
                char separater = '?';
                foreach (string paramName in parameterNames)
                {
                    if (SIGNED_PARAMTERS.Contains(paramName))
                    {
                        builder.Append(separater);
                        builder.Append(paramName);
                        string paramValue = parameters[paramName];
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

        public static string BuildCanonicalString(string resourcePath, HttpRequestMessage httpRequestMessage)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.Append(httpRequestMessage.Method).Append("\n");
            if (httpRequestMessage.Content!= null && httpRequestMessage.Content.Headers.ContentType != null)
                builder.Append(httpRequestMessage.Content.Headers.ContentType.ToString());
            builder.Append("\n");
            if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.ContentMD5 != null)
                builder.Append(httpRequestMessage.Content.Headers.ContentMD5.ToString());
            builder.Append("\n");
            builder.Append(DateUtils.FormatRfc822Date(httpRequestMessage.Headers.Date.Value.UtcDateTime));
            builder.Append("\n");
            builder.Append(resourcePath);
            return builder.ToString();



            //IDictionary<string, string> headers = httpRequestMessag;
           
            //if (headers != null)
            //{
            //    foreach (KeyValuePair<string, string> header in headers)
            //    {
            //        string lowerKey = header.Key.ToLowerInvariant();
            //        if (((lowerKey == "Content-Type".ToLowerInvariant()) || (lowerKey == "Content-MD5".ToLowerInvariant())) || ((lowerKey == "Date".ToLowerInvariant()) || lowerKey.StartsWith("x-oss-")))
            //        {
            //            headersToSign.Add(lowerKey, header.Value);
            //        }
            //    }
            //}
            //if (!headersToSign.ContainsKey("Content-Type".ToLowerInvariant()))
            //{
            //    headersToSign.Add("Content-Type".ToLowerInvariant(), "");
            //}
            //if (!headersToSign.ContainsKey("Content-MD5".ToLowerInvariant()))
            //{
            //    headersToSign.Add("Content-MD5".ToLowerInvariant(), "");
            //}
            //if (request.Parameters != null)
            //{
            //    foreach (KeyValuePair<string, string> p in request.Parameters)
            //    {
            //        if (p.Key.StartsWith("x-oss-"))
            //        {
            //            headersToSign.Add(p.Key, p.Value);
            //        }
            //    }
            //}
            //foreach (KeyValuePair<string, string> entry in from e in headersToSign
            //    orderby e.Key
            //    select e)
            //{
            //    string key = entry.Key;
            //    object value = entry.Value;
            //    if (key.StartsWith("x-oss-"))
            //    {
            //        builder.Append(key).Append(':').Append(value);
            //    }
            //    else
            //    {
            //        builder.Append(value);
            //    }
            //    builder.Append("\n");
            //}
            //builder.Append(BuildCanonicalizedResource(resourcePath, request.Parameters));
            //return builder.ToString();
        }
    }
}
