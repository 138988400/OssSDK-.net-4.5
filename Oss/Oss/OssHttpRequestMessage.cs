using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    class OssHttpRequestMessage : HttpRequestMessage
    {

        public OssHttpRequestMessage(string bucketName, string key, IDictionary<string, string> _parameters = null)
            : this(OssUtils.DefaultEndpoint, bucketName, key, _parameters)
        {
        }

        public OssHttpRequestMessage(Uri endpoint, string bucketName, string key, IDictionary<string, string> _parameters = null)
        {
            Endpoint = endpoint;
            ResourcePath = "/" + ((bucketName != null) ? bucketName : "") + ((key != null) ? ("/" + key) : "");
             parameters = _parameters;
             RequestUri = new Uri(BuildRequestUri());
        }

        public string BuildRequestUri()
        {
            string uri = this.Endpoint.ToString();
            if (!uri.EndsWith("/") && ((this.ResourcePath == null) || !this.ResourcePath.StartsWith("/")))
            {
                uri = uri + "/";
            }
            if (this.ResourcePath != null)
            {
                uri = uri + this.ResourcePath;
            }

            if (Parameters != null)
            {
                string paramString = HttpUtils.GetRequestParameterString(this.parameters);
                if (!string.IsNullOrEmpty(paramString))
                {
                    uri = uri + "?" + paramString;
                }
            }
            return uri;
        }



        private IDictionary<string, string> parameters = null;


        public IDictionary<string, string> Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public string ResourcePath { get; set; }

        public Uri Endpoint { get; set; }
    }
}
