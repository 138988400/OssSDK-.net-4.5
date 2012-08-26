using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Error>\n  <Code>SignatureDoesNotMatch</Code>\n  <Message>The request signature we calculated does not match the signature you provided. Check your key and signing method.</Message>\n  <StringToSignBytes>50 55 54 0A 0A 0A 53 75 6E 2C 20 32 36 20 41 75 67 20 32 30 31 32 20 31 32 3A 34 32 3A 32 37 20 47 4D 54 0A 2F 6D 79 64 6F 63 34 </StringToSignBytes>\n  <SignatureProvided>yDXTuEj8yX7aCOk7Emh/TlhJVrs=</SignatureProvided>\n  <StringToSign>PUT\n\n\nSun, 26 Aug 2012 12:42:27 GMT\n/mydoc4</StringToSign>\n  <OSSAccessKeyId>bm9crcnr0rtnuw8bnrfvq7w8</OSSAccessKeyId>\n  <RequestId>503A19B54DEF3F377EB51C7E</RequestId>\n  <HostId>storage.aliyun.com</HostId>\n</Error>\n"
namespace Oss
{
    class test
    {
         static void Main(string[] args)
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                temp.CreateBucket("mydoc5");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
    }
}