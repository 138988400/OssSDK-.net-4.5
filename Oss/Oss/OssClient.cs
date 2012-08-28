using Oss.Deserial;
using Oss.Model;
using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Oss
{
    public class OssClient
    {
        public HttpClient httpClient;
        public NetworkCredential networkCredential;

        public OssClient(string accessId, string accessKey)
            : this(OssUtils.DefaultEndpoint, accessId, accessKey)
        {
        }

        public OssClient(string endpoint, string accessId, string accessKey)
            : this(new Uri(endpoint), accessId, accessKey)
        {
        }

        public OssClient(Uri endpoint, string accessId, string accessKey)
        {
            if (endpoint == null)
            {

                throw new ArgumentNullException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "endpoint");
            }
            if (string.IsNullOrEmpty(accessId))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            }
            if (string.IsNullOrEmpty(accessKey))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");
            }

            networkCredential = new NetworkCredential(accessId, accessKey);
            httpClient = new HttpClient();
           // httpClient.BaseAddress = endpoint;

        }

        public async Task<Bucket> CreateBucket(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            }

            if (!OssUtils.IsBucketNameValid(bucketName))
            {
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            }


            OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName);

            httpRequestMessage.Method = HttpMethod.Put;
            httpRequestMessage.Headers.Host = "storage.aliyun.com";
            httpRequestMessage.Headers.Date = DateTime.UtcNow;

            OssRequestSigner.Sign(httpRequestMessage, networkCredential);
            HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

            if (test.IsSuccessStatusCode == false)
            {
                ErrorResponseHandler handler = new ErrorResponseHandler();
                handler.Handle(test);
            }

            return new Bucket(bucketName);
        }

        public async Task<IEnumerable<Bucket>> ListBuckets()
        {
            OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage("");

            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Host = "storage.aliyun.com";
            httpRequestMessage.Headers.Date = DateTime.UtcNow;

            OssRequestSigner.Sign(httpRequestMessage, networkCredential);
            HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

            if (test.IsSuccessStatusCode == false)
            {
                ErrorResponseHandler handler = new ErrorResponseHandler();
                handler.Handle(test);
            }

            var temp = DeserializerFactory.GetFactory().CreateListBucketResultDeserializer();
            IEnumerable<Bucket> result = await   temp.Deserialize(test);
            return result;

        }
    }
}
;