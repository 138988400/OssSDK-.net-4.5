using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    class MultipartUploadDeserializer : IDeserializer<HttpResponseMessage, MultipartUploadResult>
    {
        public MultipartUploadDeserializer()
        {
        }

        public MultipartUploadResult Deserialize(HttpResponseMessage response)
        {
            MultipartUploadResult result = new MultipartUploadResult();
            if (response.Headers.ETag.Tag != "")
            {
                result.ETag = OssUtils.TrimETag(response.Headers.ETag.Tag);
            }
            return result;
        }
    }
 
}
