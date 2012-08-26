using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

                throw new ArgumentNullException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "endpoint");
            }
            if (string.IsNullOrEmpty(accessId))
            {
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            }
            if (string.IsNullOrEmpty(accessKey))
            {
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");
            }

            networkCredential = new NetworkCredential(accessId, accessKey);
            httpClient = new HttpClient();
           // httpClient.BaseAddress = endpoint;

        }

        public async void CreateBucket(string bucketName)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Put;
                //HttpContent temp = new StringContent("");
                //httpRequestMessage.Content = temp;
                httpRequestMessage.RequestUri = new Uri(OssUtils.DefaultEndpoint + bucketName);
                httpRequestMessage.Headers.Host = "storage.aliyun.com";
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                OssRequestSigner.Sign(bucketName, httpRequestMessage, networkCredential);
                var test = await httpClient.SendAsync(httpRequestMessage);
                Console.ReadKey();
            }
            catch (Exception)
            {

            }
        }
    }
}
;