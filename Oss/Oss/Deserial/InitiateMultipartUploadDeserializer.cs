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
    internal class InitiateMultipartUploadDeserializer : ResponseDeserializer<string, InitiateMultipartUploadResultModel>
    {
        public InitiateMultipartUploadDeserializer(IDeserializer<Stream, InitiateMultipartUploadResultModel> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public async override Task<string> Deserialize(HttpResponseMessage response)
        {
            InitiateMultipartUploadResultModel listBucketResult = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());

            return listBucketResult.UploadId; 
 
        }
    }

}
