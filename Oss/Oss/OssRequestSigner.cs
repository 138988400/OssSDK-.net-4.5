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
        public static void Sign(OssHttpRequestMessage ossHttpRequestMessage, NetworkCredential networkCredential)
        {
            string accessKeyId = networkCredential.UserName;
            string secretAccessKey = networkCredential.Password;
            if (!string.IsNullOrEmpty(secretAccessKey))
            {
                string canonicalString = SignUtils.BuildCanonicalString(ossHttpRequestMessage);
                string signature = ServiceSignature.Create().ComputeSignature(secretAccessKey, canonicalString);
                ossHttpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OSS", accessKeyId + ":" + signature);

            }
            else
            {
                ossHttpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", accessKeyId);
            }


        }
    }
}
