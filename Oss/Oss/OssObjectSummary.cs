using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class OssObjectSummary
    {
        public OssObjectSummary()
        {
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[OSSObjectSummary BucketName={0}, Key={1}]", new object[] { this.BucketName, this.Key });
        }

        public string BucketName { get;  set; }

        public string ETag { get;  set; }

        public string Key { get;  set; }

        public DateTime LastModified { get;  set; }

        public Owner Owner { get;  set; }

        public long Size { get;  set; }

        public string StorageClass { get;  set; }
    }
}
