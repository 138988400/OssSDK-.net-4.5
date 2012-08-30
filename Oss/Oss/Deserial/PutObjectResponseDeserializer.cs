using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal class PutObjectResponseDeserializer : IDeserializer<HttpResponseMessage, PutObjectResult>
    {
        public PutObjectResponseDeserializer()
        {
        }

        public   PutObjectResult Deserialize(HttpResponseMessage response)
        {
            PutObjectResult result = new PutObjectResult();
            if (response.Headers.ETag.Tag != "")
            {
                result.ETag = OssUtils.TrimETag(response.Headers.ETag.Tag);
            }
            return result;
        }
    }
}
