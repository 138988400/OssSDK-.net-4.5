using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    class test
    {
         static void Main(string[] args)
        {
            try
            {
                OssClient temp = new OssClient("bm9crcnr0rtnuw8bnrfvq7w8", "RbtJoExTnA8vYLynUfDh7Ior+oM=");
                temp.CreateBucket("mydoc4");
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