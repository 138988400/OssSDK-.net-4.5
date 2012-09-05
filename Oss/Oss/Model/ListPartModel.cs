using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Model
{

    [Serializable]
    public class ListPartModel
    {

        public UInt32 PartNumber { get; set; }

        public string ETag { get; set; }
        public UInt32 Size { get; set; }

        public DateTime LastModified { get; set; }
    }
}
