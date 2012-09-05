using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Model
{
    [Serializable]
    public class ListMultiPartUploadModle
    {
        public string Key { get; set; }

        public string UploadId { get; set; }
        public DateTime Initiated { get; set; }
    }
}
