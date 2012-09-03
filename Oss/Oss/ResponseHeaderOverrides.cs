using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class ResponseHeaderOverrides
    {
        internal const string ResponseCacheControl = "response-cache-control";
        internal const string ResponseContentDisposition = "response-content-disposition";
        internal const string ResponseContentEncoding = "response-content-encoding";
        internal const string ResponseHeaderContentLanguage = "response-content-language";
        internal const string ResponseHeaderContentType = "response-content-type";
        internal const string ResponseHeaderExpires = "response-expires";

        internal void Populate(HttpRequestHeaders headers)
        {
            if (this.CacheControl != null)
            {
                headers.Add("response-cache-control", this.CacheControl);
            }
            if (this.ContentDisposition != null)
            {
                headers.Add("response-content-disposition", this.ContentDisposition);
            }
            if (this.ContentEncoding != null)
            {
                headers.Add("response-content-encoding", this.ContentEncoding);
            }
            if (this.ContentLanguage != null)
            {
                headers.Add("response-content-language", this.ContentLanguage);
            }
            if (this.ContentType != null)
            {
                headers.Add("response-content-type", this.ContentType);
            }
            if (this.Expires != null)
            {
                headers.Add("response-expires", this.Expires);
            }
        }

        public string CacheControl { get; set; }

        public string ContentDisposition { get; set; }

        public string ContentEncoding { get; set; }

        public string ContentLanguage { get; set; }

        public string ContentType { get; set; }

        public string Expires { get; set; }
    }
}
