 OssSDK  .Net 4.5

一 简介

用C#做客户端时，发现官网上发布的.Net SDK 不太好用。正逢微软发布vs2012和 .net4.5，在原来的.net sdk代码基础上重写了SDK。接口与原来的基本保持一致。
新SDK API基于TPL (Task Parallel Library)。

二 新增功能

1 所有API均为异步，可以用async, await很方便的调用异步操作请求，
且效率很高。对客户端UI的操作平滑性有很好的支持。
2 在put和get object接口中添加HttpProcess callback的Action参数，可以回调下载以及上传的数据进度。（Percent, BytesTransferred, TotalBytes）
3 在put和get object接口中添加 CancellationToken参数，可以取消上传或下载的任务。4 增加MultiPart Upload的API，支持分块上传。

三 源代码网络地址

https://github.com/ZhongleiYang/sdk
9月初我在社区开源过这个SDK源代http://bbs.aliyun.com/read.php?tid=120577&fpage=5
后来因为客户端代码的关系，改变了源代码地址。

四 API

API位于命名空间位于Oss. OssClient，调用API成功则返回，错误抛异常。
所有API如下：

        //创建OssClient对象
        public OssClient(string accessId, string accessKey);           
        public OssClient(string endpoint, string accessId, string accessKey);
        public OssClient(Uri endpoint, string accessId, string accessKey);
  
        //创建Bucket
        public async Task<Bucket> CreateBucket(string bucketName);

        //删除Bucket
        public async Task DeleteBucket(string bucketName);
   
       //获得Bucket的权限
        public async Task<AccessControlList> GetBucketAcl(string bucketName)
       
        //设置Bucket的权限
        public async Task SetBucketAcl(string bucketName, CannedAccessControlList acl);

        //获得所有的Bucket
        public async Task<IEnumerable<Bucket>> ListBuckets();
        
        //上传Object
        public async Task<PutObjectResult> PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata, 
            Action<HttpProcessData> uploadProcessCallback = null, CancellationToken? cancellationToken = null);
     

        //列举Objects
       public async Task<ObjectListing> ListObjects(string bucketName);
       public async Task<ObjectListing> ListObjects(string bucketName, string prefix);
       public async Task<ObjectListing> ListObjects(ListObjectsRequest listObjectsRequest);
     
        //下载Object
        public async Task<OssObject> GetObject(string bucketName, string key, 
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null);

        public async Task<OssObject> GetObject(GetObjectRequest getObjectRequest,
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null);
       
        public async Task<ObjectMetadata> GetObject(GetObjectRequest getObjectRequest, Stream output,
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null);
        
        //获得Object的MetaData
        public async  Task<ObjectMetadata> GetObjectMetadata(string bucketName, string key);
       
       //删除Object
        public async Task DeleteObject(string bucketName, string key);
     
       //初始化多块上传，返回updateId
        public async Task<string> MultipartUploadInitiate(string bucketName, string key);
    
        //分块上传
        public async Task<MultipartUploadResult> MultipartUpload(MultiUploadRequestData multiUploadObject,
            Action<HttpProcessData> uploadProcessCallback = null, CancellationToken? cancellationToken = null);
       
        //完成上传
        public async Task<CompleteMultipartUploadResult> CompleteMultipartUpload(CompleteMultipartUploadModel completeMultipartUploadModel);
   
       //删除多块上传任务
        public async Task DeleteMultipartUpload(MultiUploadRequestData multiUploadObject);
     
       //获得分块上传的结果
       public async Task<ListPartsResult> ListMultiUploadParts(string buketName, string key, string uploadId);
        
        //获得Bucket所有分块上传的结果
        public async Task<ListMultipartUploadsResult> ListMultipartUploads(string bucketName);
        
五 主要API说明及用例（其他参照原先SDK说明和OSSAPI的说明）

1.
public async Task<OssObject> GetObject(GetObjectRequest getObjectRequest,
            Action<HttpProcessData> downloadProcessCallback = null, CancellationToken? cancellationToken = null)
用途：下载Objet
用例：这段代码在调用getOject()时，下载40%的时候，取消任务。

    CancellationTokenSource tokenSource = new CancellationTokenSource();
    void callback(HttpProcessData processPercent)
    {
        if (processPercent.ProgressPercentage == 40)
        {
             	tokenSource.Cancel();
        }
    }

     void getObject()
     {
         FileStream fs = null;
         Stream stream = null;
         try
         {   
             OssObject result = await temp.GetObject("devdoc", "c# 5.0.pdf", callback);
             stream = obj.Content;
             fs = new FileStream(fileName, FileMode.OpenOrCreate);
             await stream.CopyToAsync(fs);
             fs.Position = 0;
             fs.Flush();
         }
         catch (Exception ex)
         {
              throw ex;
         }
         finally
         {
             if(fs != null)
                fs.Close();
     
             if (stream != null)
                 stream.Close();
         }
     
     }

2．多块上传示例

这段代码调用MultipartUploadSample，实现对文件c# 5.0.pdf 分2块上传
        static int ReadChunk(Stream stream, byte[] chunk)
        {
            int index = 0;
            while (index < chunk.Length)
            {
                int bytesRead = stream.Read(chunk, index, chunk.Length - index);
                if (bytesRead == 0)
                {
                    break;
                }
                index += bytesRead;
            }
            return index;
        }



        static async void MultipartUploadSample ()
        {
            try
            {
                OssClient client = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                string uploadId = await client.MultipartUploadInitiate("devdoc", "c# 5.0.pdf");

                FileStream fs = new FileStream(@"c# 5.0.pdf", FileMode.Open);

                byte[] buffer = new byte[6291456];

                ReadChunk(fs, buffer);

                MemoryStream ms = new MemoryStream(buffer);

                MultiUploadRequestData arg = new MultiUploadRequestData() { Bucket = "devdoc", Key = "c# 5.0.pdf", Content = ms, PartNumber = "1", UploadId = result };
                MultipartUploadResult result1 = await client.MultipartUpload(arg, callback);

                //ListMultipartUploadsResult listMultipart = await client.ListMultipartUploads("devdoc");

                //client.DeleteMultipartUpload(arg);

               fs.Position = 6291456;
               arg = new MultiUploadRequestData() { Bucket = "devdoc", Key = "c# 5.0.pdf", Content = fs, PartNumber = "2", UploadId = uploadId };
               MultipartUploadResult result2 = await client.MultipartUpload(arg);

               ListPartsResult parts = await client.ListMultiUploadParts("devdoc", "c# 5.0.pdf", uploadId);


               CompleteMultipartUploadModel model = new CompleteMultipartUploadModel();

               model.Parts = new List<MultipartUploadPartModel>();
               model.Parts.Add(new MultipartUploadPartModel(1, result1.ETag));
               model.Parts.Add(new MultipartUploadPartModel(2, result2.ETag));
               model.Bucket = "devdoc";
               model.Key = "c# 5.0.pdf";
               model.UploadId = uploadId;


               await client.CompleteMultipartUpload(model);

               fs.Dispose();

            }
            catch (AggregateException ex)
            {
                throw ex;

            }
        }
