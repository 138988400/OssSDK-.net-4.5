using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class ListObjectsRequest
    {
        public ListObjectsRequest(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            }
            if (!OssUtils.IsBucketNameValid(bucketName))
            {
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            }
            this.BucketName = bucketName;
        }

        public string BucketName { get; private set; }

        public string Delimiter { get; set; }

        public string Marker { get; set; }

        public int? MaxKeys { get; set; }

        public string Prefix { get; set; }
    }
}
