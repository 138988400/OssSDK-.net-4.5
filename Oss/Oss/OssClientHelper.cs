using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    internal class OssClientHelper
    {
        public static void initialHttpRequestMessage(OssHttpRequestMessage hReqMes, ObjectMetadata metadata)
        {
            if (metadata.CacheControl != null)
            {
                CacheControlHeaderValue temp;
                if (CacheControlHeaderValue.TryParse(metadata.CacheControl, out temp) == true)
                    hReqMes.Headers.CacheControl = temp;
            }

            if (metadata.ContentDisposition != null)
            {
                ContentDispositionHeaderValue temp;
                if (ContentDispositionHeaderValue.TryParse(metadata.CacheControl, out temp) == true)
                    hReqMes.Content.Headers.ContentDisposition = temp;
            }

            if (metadata.ContentLength != 0)
            {
                hReqMes.Content.Headers.ContentLength = metadata.ContentLength;
            }

            if (metadata.ContentType != null)
            {
                MediaTypeHeaderValue temp;
                if (MediaTypeHeaderValue.TryParse(metadata.CacheControl, out temp) == true)
                    hReqMes.Content.Headers.ContentType = temp;
                else
                    hReqMes.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            }
            else
            {
                hReqMes.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }

            //if (metadata.ExpirationTime != null)
            //{
            //    hReqMes.Content.Headers.Expires = metadata.ExpirationTime;
            //}

            foreach (KeyValuePair<string, string> entry in metadata.UserMetadata)
            {
                hReqMes.Parameters.Add("x-oss-meta-" + entry.Key, entry.Value);
            }



        }
    }
}
