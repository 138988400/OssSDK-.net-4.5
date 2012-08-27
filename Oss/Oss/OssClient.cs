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

        public async void CreateBucket(string bucketName)
        {
            try
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
                //HttpContent temp = new StringContent("");
                //httpRequestMessage.Content = temp;
               // httpRequestMessage.RequestUri = new Uri(OssUtils.DefaultEndpoint + bucketName);
                httpRequestMessage.Headers.Host = "storage.aliyun.com";
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                  //var temp =  DeserializerFactory.GetFactory().CreateErrorResultDeserializer();
                  //ErrorResult error = await temp.Deserialize(test);
                }


                string result = await test.Content.ReadAsStringAsync();

                Console.ReadKey();
            }
            catch (Exception)
            {

            }
        }
    }
}
;