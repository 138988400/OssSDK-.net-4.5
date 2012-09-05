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

        public async void DeleteBucket(string bucketName)
        {
            try
            {
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, null);

                httpRequestMessage.Method = HttpMethod.Delete;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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


        public async Task<ObjectListing> ListObjects(string bucketName)
        {
            return await this.ListObjects(bucketName, null);
        }

        public async Task<ObjectListing> ListObjects(string bucketName, string prefix)
        {
            ListObjectsRequest temp = new ListObjectsRequest(bucketName)
            {
                Prefix = prefix
            };
            return await  this.ListObjects(temp);
        }


        public async Task<ObjectListing> ListObjects(ListObjectsRequest listObjectsRequest)
        {
            ObjectListing result = null;

            try
            {

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(listObjectsRequest.BucketName, null);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }

                var temp = DeserializerFactory.GetFactory().CreateListObjectsResultDeserializer();
                result = await temp.Deserialize(test);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<OssObject> GetObject(string bucketName, string key)
        {
            return await  this.GetObject(new GetObjectRequest(bucketName, key));
        }

        public async Task<OssObject> GetObject(GetObjectRequest getObjectRequest)
        {

            OssObject result = null;

            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(getObjectRequest.BucketName, getObjectRequest.Key);
                getObjectRequest.ResponseHeaders.Populate(httpRequestMessage.Headers);
                getObjectRequest.Populate(httpRequestMessage.Headers);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);

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

                var temp = DeserializerFactory.GetFactory().CreateGetObjectResultDeserializer(getObjectRequest);
                result = await temp.Deserialize(test);
            }
            catch (Exception ex)
            {

            }

            return result;   
        }

        public async Task<ObjectMetadata> GetObject(GetObjectRequest getObjectRequest, Stream output)
        {
            OssObject ossObject = await this.GetObject(getObjectRequest);
            using (ossObject.Content)
            {
                ossObject.Content.CopyTo(output);
            }
            return ossObject.Metadata;
        }


        public async  Task<ObjectMetadata> GetObjectMetadata(string bucketName, string key)
        {
            ObjectMetadata result = null;

            try
            {

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key);

                httpRequestMessage.Method = HttpMethod.Head;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }

                var temp = DeserializerFactory.GetFactory().CreateGetObjectMetadataResultDeserializer();
                result = temp.Deserialize(test);
            }
            catch (Exception ex)
            {

            }
            return result;
            
        }

        public async void DeleteObject(string bucketName, string key)
        {
            try
            {
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key);

                httpRequestMessage.Method = HttpMethod.Delete;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<string> MultipartUploadInitiate(string bucketName, string key)
        {
            string result = null;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("uploads", null);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key, parameters);

                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
                var temp = DeserializerFactory.GetFactory().CreateInitiateMultipartUploadDeserializer();
                result = await temp.Deserialize(test);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }

        public async Task<MultipartUploadResult> MultipartUpload(MultiUploadRequestData multiUploadObject)
        {
            MultipartUploadResult result = null;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("partNumber", multiUploadObject.PartNumber);
                parameters.Add("uploadId", multiUploadObject.UploadId);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(multiUploadObject.Bucket, multiUploadObject.Key, parameters);

                httpRequestMessage.Method = HttpMethod.Put;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                httpRequestMessage.Content = new StreamContent(multiUploadObject.Content);

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
                var temp = DeserializerFactory.GetFactory().CreateMultipartUploadDeserializer();
                result = temp.Deserialize(test);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }

        public async Task<CompleteMultipartUploadResult> CompleteMultipartUpload(CompleteMultipartUploadModel completeMultipartUploadModel)
        {
            CompleteMultipartUploadResult result = null;
            try
            {

               
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("uploadId", completeMultipartUploadModel.UploadId);


                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(completeMultipartUploadModel.Bucket, completeMultipartUploadModel.Key, parameters);

                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                XmlStreamSerializer<CompleteMultipartUploadModel> serializer = new XmlStreamSerializer<CompleteMultipartUploadModel>();
              //  FileStream fileStream = new FileStream("1.xml", FileMode.Open);

                //httpRequestMessage.Content = new StreamContent(fileStream);
                httpRequestMessage.Content = new StreamContent(serializer.Serialize(completeMultipartUploadModel));

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
                var temp = DeserializerFactory.GetFactory().CreateCompMultiUploadDeserializer();
                result = await temp.Deserialize(test);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }


        public async void DeleteMultipartUpload(MultiUploadRequestData multiUploadObject)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("partNumber", multiUploadObject.PartNumber);
                parameters.Add("uploadId", multiUploadObject.UploadId);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(multiUploadObject.Bucket, multiUploadObject.Key, parameters);

                httpRequestMessage.Method = HttpMethod.Delete;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<ListPartsResult> ListMultiUploadParts(string buketName, string key, string uploadId)
        {
            ListPartsResult result = null;
            try
            {


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("uploadId", uploadId);


                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(buketName, key, parameters);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage test = await httpClient.SendAsync(httpRequestMessage);

                if (test.IsSuccessStatusCode == false)
                {
                    ErrorResponseHandler handler = new ErrorResponseHandler();
                    handler.Handle(test);
                }
                var temp = DeserializerFactory.GetFactory().CreateListPartsDeserialzer();
                result = await temp.Deserialize(test);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public async Task<ListMultipartUploadsResult> ListMultipartUploads(string bucketName)
        {
            ListMultipartUploadsResult result = null;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("uploads", null);

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
                var temp = DeserializerFactory.GetFactory().CreateListMultipartUploadsDeserializer();
                result = await temp.Deserialize(test);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }

    }
}