using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    internal class OssRequestSigner
    {
        public static void Sign(string resourcePath, HttpRequestMessage httpRequestMessage, NetworkCredential networkCredential)
        {
            string accessKeyId = networkCredential.UserName;
            string secretAccessKey = networkCredential.Password;
            if (!string.IsNullOrEmpty(secretAccessKey))
            {
                string canonicalString = SignUtils.BuildCanonicalString(resourcePath, httpRequestMessage);
                string signature = ServiceSignature.Create().ComputeSignature(secretAccessKey, canonicalString);
                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OSS", accessKeyId + ":" + signature);

            }
            else
            {
                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", accessKeyId);
            }


        }
    }
}
