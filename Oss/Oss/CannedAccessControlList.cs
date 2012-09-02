using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public enum CannedAccessControlList
    {
        [StringValue("private")]
        Private = 0,
        [StringValue("public-read")]
        PublicRead = 1,
        [StringValue("public-read-write")]
        PublicReadWrite = 2
    }
}
