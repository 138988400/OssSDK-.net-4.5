using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Error>\n  <Code>SignatureDoesNotMatch</Code>\n  <Message>The request signature we calculated does not match the signature you provided. Check your key and signing method.</Message>\n  <StringToSignBytes>50 55 54 0A 0A 0A 53 75 6E 2C 20 32 36 20 41 75 67 20 32 30 31 32 20 31 32 3A 34 32 3A 32 37 20 47 4D 54 0A 2F 6D 79 64 6F 63 34 </StringToSignBytes>\n  <SignatureProvided>yDXTuEj8yX7aCOk7Emh/TlhJVrs=</SignatureProvided>\n  <StringToSign>PUT\n\n\nSun, 26 Aug 2012 12:42:27 GMT\n/mydoc4</StringToSign>\n  <OSSAccessKeyId>bm9crcnr0rtnuw8bnrfvq7w8</OSSAccessKeyId>\n  <RequestId>503A19B54DEF3F377EB51C7E</RequestId>\n  <HostId>storage.aliyun.com</HostId>\n</Error>\n"
namespace Oss
{
    class test
    {
        static async void PutObject()
        {
            OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
            FileStream fs = new FileStream(@"C:\Users\yangzhl\Desktop\c# 5.0.pdf", FileMode.Open);
            ObjectMetadata oMetaData= new ObjectMetadata();
            await temp.PutObject("devdoc", "c# 5.0.pdf", fs, oMetaData);
            fs.Dispose();
        }


        static async void list()
        {
            OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
            IEnumerable<Bucket> test = await temp.ListBuckets();
        }


        static void createBuket()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                Task<Bucket> test = temp.CreateBucket("mydoc10");

            }
            catch (AggregateException ex)
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

        static void setBuketAcl()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                temp.SetBucketAcl("mydoc4", CannedAccessControlList.PublicRead);

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static void deleteBuket()
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                temp.DeleteBucket("mydoc5");

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
             //   list();
               // createBuket();
                //PutObject();
               // getBuketAcl();
                deleteBuket();
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