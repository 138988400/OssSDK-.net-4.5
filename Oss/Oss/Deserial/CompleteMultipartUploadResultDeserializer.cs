using Oss.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    class CompleteMultipartUploadResultDeserializer : ResponseDeserializer<CompleteMultipartUploadResult, CompleteMultipartUploadResult>
    {
        public CompleteMultipartUploadResultDeserializer(IDeserializer<Stream, CompleteMultipartUploadResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override async Task<CompleteMultipartUploadResult> Deserialize(HttpResponseMessage response)
        {
            CompleteMultipartUploadResult model = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            return model;
        }

    }
}
