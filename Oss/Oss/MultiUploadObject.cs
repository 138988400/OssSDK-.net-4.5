using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class MultiUploadObject
    {
        public string Bucket { get; set; }
        public string Key { get; set; }
        public string UploadId { get; set; }
        public string PartNumber { get; set; }
        public Stream Content { get; set; }
    }
}
