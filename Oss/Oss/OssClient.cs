using Oss.Commands;
using Oss.Deserial;
using Oss.Model;
using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Net.Http.Headers;
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
            try{
                      

            bool result = await CreateBucketCommand.Create(httpClient, bucketName, networkCredential).Execute();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
            return new Bucket(bucketName);
        }

        public async Task<IEnumerable<Bucket>> ListBuckets()
        {
            IEnumerable<Bucket> result = null;
            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(null, null);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);


                processMessageHander.HttpSendProgress += (sender, e) =>
                    {
                        int num = e.ProgressPercentage;
                     //   Console.WriteLine(num);

                    };
                processMessageHander.HttpReceiveProgress += (sender, e) =>
                {
                    int num = e.ProgressPercentage;
                   // Console.WriteLine(num);

                };

                HttpResponseMessage test = await localHttpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }

                var temp = DeserializerFactory.GetFactory().CreateListBucketResultDeserializer();
                result = await temp.Deserialize(test);
                localHttpClient.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;

        }

        public async Task <PutObjectResult> PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata)
        {
            PutObjectResult result = null;
            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key);



                httpRequestMessage.Method = HttpMethod.Put;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                httpRequestMessage.Content = new StreamContent(content);


                OssClientHelper.initialHttpRequestMessage(httpRequestMessage, metadata);

               
                

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);


                processMessageHander.HttpSendProgress += (sender, e) =>
                {
                    int num = e.ProgressPercentage;
                    //   Console.WriteLine(num);

                };
                processMessageHander.HttpReceiveProgress += (sender, e) =>
                {
                    int num = e.ProgressPercentage;
                    // Console.WriteLine(num);

                };

                HttpResponseMessage test = await localHttpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }

                var temp = DeserializerFactory.GetFactory().CreatePutObjectReusltDeserializer();
                result = temp.Deserialize(test);
                localHttpClient.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;

        }

        public async Task<AccessControlList> GetBucketAcl(string bucketName)
        {
            AccessControlList result  = null;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("acl", null);
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, null, parameters);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
                var temp = DeserializerFactory.GetFactory().CreateGetAclResultDeserializer();
                result = await temp.Deserialize(test);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;


        }


        public async void SetBucketAcl(string bucketName, CannedAccessControlList acl)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("acl", null);
            OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, null,parameters);

            httpRequestMessage.Method = HttpMethod.Put;
            httpRequestMessage.Headers.Date = DateTime.UtcNow;
            httpRequestMessage.Headers.Add("x-oss-acl", acl.GetStringValue());
            OssRequestSigner.Sign(httpRequestMessage, networkCredential);
            HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

            if (test.IsSuccessStatusCode == false)
            {
                ErrorResponseHandler handler = new ErrorResponseHandler();
                handler.Handle(test);
            }        
        }

    }
}