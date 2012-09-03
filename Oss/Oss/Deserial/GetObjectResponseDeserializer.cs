using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal class GetObjectResponseDeserializer : ResponseDeserializer<OssObject, OssObject>
    {
        private GetObjectRequest _getObjectRequest;

        public GetObjectResponseDeserializer(GetObjectRequest getObjectRequest)
            : base(null)
        {
            this._getObjectRequest = getObjectRequest;
        }

        public override async Task<OssObject> Deserialize(HttpResponseMessage response)
        {
            return new OssObject(this._getObjectRequest.Key) 
            { 
                BucketName = this._getObjectRequest.BucketName, 
                Content = await response.Content.ReadAsStreamAsync(), 
                Metadata = DeserializerFactory.GetFactory().CreateGetObjectMetadataResultDeserializer().Deserialize(response) 
            };
        }
    }
}
