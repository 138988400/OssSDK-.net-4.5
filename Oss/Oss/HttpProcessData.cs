using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
     public  class HttpProcessData
    {
         public int ProgressPercentage { get; internal set; }
        // 摘要:
        //     获取在 HTTP 进度中传输的字节数。
        //
        // 返回结果:
        //     在 HTTP 进度中传输的字节数。
        public int BytesTransferred { get; internal set; }
        //
        // 摘要:
        //     获取由 HTTP 进度传输的字节总数。
        //
        // 返回结果:
        //     由 HTTP 进度传输的字节总数。
        public long? TotalBytes { get; internal set; }
    }
}
