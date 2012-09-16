using Oss.Deserial;
using Oss.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Oss
{
    class test
    {
        static CancellationTokenSource tokenSource = new CancellationTokenSource();
        static void callback(HttpProcessData processPercent)
        {
            if (processPercent.ProgressPercentage == 40)
            {
                tokenSource.Cancel();
            }
        }



        static async void PutObject()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                FileStream fs = new FileStream(@"C:\Users\zhonglei\Desktop\test.pdf", FileMode.Open);
               // MemoryStream s = new MemoryStream();
                ObjectMetadata oMetaData = new ObjectMetadata();
                await temp.PutObject("devdoc", "2/test.pdf", fs, oMetaData, null, tokenSource.Token);
                fs.Dispose();
            }
            catch(Exception ex)
            {

            }
        }


        static async void list()
        {
            OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
            IEnumerable<Bucket> test = await temp.ListBuckets();
        }


        static async void createBuket()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                Bucket test = await temp.CreateBucket("1");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async void getBuketAcl()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                AccessControlList test = await temp.GetBucketAcl("mydoc5");

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async void setBuketAcl()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                await temp.SetBucketAcl("mydoc4", CannedAccessControlList.PublicRead);

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async void deleteBuket()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                await temp.DeleteBucket("mydoc12");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async void listObjects()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                ListObjectsRequest arg = new ListObjectsRequest("devdoc2");
               arg.Delimiter=@"/";
               // arg.Prefix = @".NET Reflector 7.0.0.420 Crack/"
                ObjectListing result = await temp.ListObjects(arg );

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }


        static async void getObject()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                OssObject result = await temp.GetObject("devdoc", "c# 5.0.pdf", callback);

                

                  
                FileStream fs = new FileStream(@"C:\Users\yangzhl\Desktop\c# 5.0.pdf", FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                result.Content.Read(buffer, 0, buffer.Length);
                byte[] sh = MD5.Create().ComputeHash(buffer);
               string hashCode = BitConverter.ToString(sh).Replace("-", string.Empty).ToLower();


            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async void getObjectMeta()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
               ObjectMetadata result = await temp.GetObjectMetadata("devdoc", "c# 5.0.pdf");

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static  async void deleteObject()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                await temp.DeleteObject("devdoc", "c# 5.0.pdf");

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

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



        static async void MultipartUploadInitiate()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                string result = await temp.MultipartUploadInitiate("devdoc", "c# 5.0.pdf");

                FileStream fs = new FileStream(@"c# 5.0.pdf", FileMode.Open);

                byte[] buffer = new byte[6291456];

                ReadChunk(fs, buffer);

                MemoryStream ms = new MemoryStream(buffer);

                MultiUploadRequestData arg = new MultiUploadRequestData() { Bucket = "devdoc", Key = "c# 5.0.pdf", Content = ms, PartNumber = "1", UploadId = result };
                MultipartUploadResult result1 = await temp.MultipartUpload(arg, callback);

               ListMultipartUploadsResult  listMultipart  = await temp.ListMultipartUploads("devdoc");

              //  temp.DeleteMultipartUpload(arg);

                //fs.Position = 6291456;
                //arg = new MultiUploadRequestData() { Bucket = "devdoc", Key = "c# 5.0.pdf", Content = fs, PartNumber = "2", UploadId = result };
                //MultipartUploadResult result2 = await temp.MultipartUpload(arg);

                //ListPartsResult parts = await temp.ListMultiUploadParts("devdoc", "c# 5.0.pdf", result);


                //CompleteMultipartUploadModel model = new CompleteMultipartUploadModel();

                //model.Parts = new List<MultipartUploadPartModel>();
                //model.Parts.Add(new MultipartUploadPartModel(1, result1.ETag));
                //model.Parts.Add(new MultipartUploadPartModel(2, result2.ETag));
                //model.Bucket = "devdoc";
                //model.Key = "c# 5.0.pdf";
                //model.UploadId = result;


                //temp.CompleteMultipartUpload(model);

              //  fs.Dispose();

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static void Main(string[] args)
        {
            try
            {








              //  MultipartUploadInitiate();
               // deleteObject();
               // getObject();
             //  listObjects();
             //   list();
                //createBuket();
               PutObject();
               // getBuketAcl();
            //    deleteBuket();
               // setBuketAcl();
               // getBuketAcl();
                // OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                //Bucket test =  temp.CreateBucket("mydoc10");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }




        }
    }
}