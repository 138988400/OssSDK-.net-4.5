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
using System.Threading;
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

                OssHttpRequestMessage ossHttpRequestMessage = new OssHttpRequestMessage(bucketName, null);

                ossHttpRequestMessage.Method = HttpMethod.Put;
                ossHttpRequestMessage.Headers.Date = DateTime.UtcNow;
                OssRequestSigner.Sign(ossHttpRequestMessage, networkCredential);
                HttpResponseMessage response = await httpClient.SendAsync(ossHttpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new Bucket(bucketName);
        }

        public async Task DeleteBucket(string bucketName)
        {
            try
            {
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, null);

                httpRequestMessage.Method = HttpMethod.Delete;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<Bucket>> ListBuckets()
        {
            IEnumerable<Bucket> result = null;
            try
            {
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(OssHttpRequestMessage.NONEEDBUKETNAME, null);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);


                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }

                var temp = DeserializerFactory.GetFactory().CreateListBucketResultDeserializer();
                result = await temp.Deserialize(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }

        public async Task<PutObjectResult> PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata, 
            Action<HttpProcessData> uploadProcessCallback = null, CancellationToken? cancellationToken = null)
        {
            PutObjectResult result = null;
            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);
                localHttpClient.Timeout += new TimeSpan(2 * TimeSpan.TicksPerHour); 
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key);



                httpRequestMessage.Method = HttpMethod.Put;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                httpRequestMessage.Content = new StreamContent(content);


                OssClientHelper.initialHttpRequestMessage(httpRequestMessage, metadata);

                

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);

                if (uploadProcessCallback != null)
                {
                    processMessageHander.HttpSendProgress += (sender, e) =>
                    {
                        uploadProcessCallback(new HttpProcessData()
                        {
                            TotalBytes = e.TotalBytes,
                            BytesTransferred = e.BytesTransferred,
                            ProgressPercentage = e.ProgressPercentage
                        });

                    };
                }

                HttpResponseMessage response;
                if(cancellationToken != null)
                    response = await localHttpClient.SendAsync(httpRequestMessage, (CancellationToken)cancellationToken);
                else
                     response = await localHttpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }

                var temp = DeserializerFactory.GetFactory().CreatePutObjectReusltDeserializer();
                result = temp.Deserialize(response);
                //localHttpClient.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var temp = DeserializerFactory.GetFactory().CreateGetAclResultDeserializer();
                result = await temp.Deserialize(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;


        }


        public async Task SetBucketAcl(string bucketName, CannedAccessControlList acl)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("acl", null);
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, null,parameters);

                httpRequestMessage.Method = HttpMethod.Put;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                httpRequestMessage.Headers.Add("x-oss-acl", acl.GetStringValue());
                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if (listObjectsRequest.Prefix != null)
                {
                    parameters.Add("prefix", listObjectsRequest.Prefix);

                }
                if (listObjectsRequest.Delimiter != null)
                {
                    parameters.Add("delimiter", listObjectsRequest.Delimiter);

                }
                if (listObjectsRequest.Marker != null)
                {
                    parameters.Add("marker", listObjectsRequest.Marker);

                }
                if (listObjectsRequest.MaxKeys != null)
                {
                    parameters.Add("maxKeys", listObjectsRequest.MaxKeys.ToString());

                }

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(listObjectsRequest.BucketName, null, parameters);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }

                var temp = DeserializerFactory.GetFactory().CreateListObjectsResultDeserializer();
                result = await temp.Deserialize(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<OssObject> GetObject(string bucketName, string key, 
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null)
        {
            return await this.GetObject(new GetObjectRequest(bucketName, key), downloadProcessCallback, cancellationToken);
        }

        public async Task<OssObject> GetObject(GetObjectRequest getObjectRequest,
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null)
        {

            OssObject result = null;

            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);
                localHttpClient.Timeout += new TimeSpan(2 * TimeSpan.TicksPerHour); 

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(getObjectRequest.BucketName, getObjectRequest.Key);
                getObjectRequest.ResponseHeaders.Populate(httpRequestMessage.Headers);
                getObjectRequest.Populate(httpRequestMessage.Headers);

                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                if (downloadProcessCallback != null)
                {
                    processMessageHander.HttpReceiveProgress += (sender, e) =>
                    {
                        downloadProcessCallback(new HttpProcessData()
                        {
                            TotalBytes = e.TotalBytes,
                            BytesTransferred = e.BytesTransferred,
                            ProgressPercentage = e.ProgressPercentage
                        }); ;

                    };
                }

                HttpResponseMessage response;
                if (cancellationToken != null)
                    response = await localHttpClient.SendAsync(httpRequestMessage, (CancellationToken)cancellationToken);
                else
                    response = await localHttpClient.SendAsync(httpRequestMessage);


                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }

                var temp = DeserializerFactory.GetFactory().CreateGetObjectResultDeserializer(getObjectRequest);
                result = await temp.Deserialize(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;   
        }

        public async Task<ObjectMetadata> GetObject(GetObjectRequest getObjectRequest, Stream output,
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null)
        {
            OssObject ossObject = await this.GetObject(getObjectRequest, downloadProcessCallback, cancellationToken);
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }

                var temp = DeserializerFactory.GetFactory().CreateGetObjectMetadataResultDeserializer();
                result = temp.Deserialize(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
            
        }

        public async Task DeleteObject(string bucketName, string key)
        {
            try
            {
                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(bucketName, key);

                httpRequestMessage.Method = HttpMethod.Delete;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;

                OssRequestSigner.Sign(httpRequestMessage, networkCredential);
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var temp = DeserializerFactory.GetFactory().CreateInitiateMultipartUploadDeserializer();
                result = await temp.Deserialize(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public async Task<MultipartUploadResult> MultipartUpload(MultiUploadRequestData multiUploadObject,
            Action<HttpProcessData> uploadProcessCallback = null, CancellationToken? cancellationToken = null)
        {
            MultipartUploadResult result = null;
            try
            {
                HttpClientHandler hand = new HttpClientHandler();
                ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);
                HttpClient localHttpClient = new HttpClient(processMessageHander);
                localHttpClient.Timeout += new TimeSpan(2 * TimeSpan.TicksPerHour); 

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("partNumber", multiUploadObject.PartNumber);
                parameters.Add("uploadId", multiUploadObject.UploadId);

                OssHttpRequestMessage httpRequestMessage = new OssHttpRequestMessage(multiUploadObject.Bucket, multiUploadObject.Key, parameters);

                httpRequestMessage.Method = HttpMethod.Put;
                httpRequestMessage.Headers.Date = DateTime.UtcNow;
                httpRequestMessage.Content = new StreamContent(multiUploadObject.Content);

                if (uploadProcessCallback != null)
                {
                    processMessageHander.HttpSendProgress += (sender, e) =>
                    {
                        uploadProcessCallback(new HttpProcessData()
                        {
                            TotalBytes = e.TotalBytes,
                            BytesTransferred = e.BytesTransferred,
                            ProgressPercentage = e.ProgressPercentage
                        });

                    };
                }


                OssRequestSigner.Sign(httpRequestMessage, networkCredential);

                HttpResponseMessage response;
                if (cancellationToken != null)
                    response = await localHttpClient.SendAsync(httpRequestMessage, (CancellationToken)cancellationToken);
                else
                    response = await localHttpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var deseserializer = DeserializerFactory.GetFactory().CreateMultipartUploadDeserializer();
                result = deseserializer.Deserialize(response);

            }
            catch (Exception ex)
            {
                throw ex;
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var temp = DeserializerFactory.GetFactory().CreateCompMultiUploadDeserializer();
                result = await temp.Deserialize(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }


        public async Task DeleteMultipartUpload(MultiUploadRequestData multiUploadObject)
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var temp = DeserializerFactory.GetFactory().CreateListPartsDeserialzer();
                result = await temp.Deserialize(response);

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
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode == false)
                {
                    await ErrorResponseHandler.Handle(response);
                }
                var temp = DeserializerFactory.GetFactory().CreateListMultipartUploadsDeserializer();
                result = await temp.Deserialize(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

    }
}