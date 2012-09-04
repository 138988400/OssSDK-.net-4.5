using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [Serializable]
    public class MultipartUploadPartModel
    {
       public  MultipartUploadPartModel()
        {
        }

       public MultipartUploadPartModel(UInt32 _PartNumber, string _ETag)
       {
           PartNumber = _PartNumber;
           ETag = _ETag;
       }

        [XmlAttribute("PartNumber")]
        public UInt32 PartNumber { get; set; }

        [XmlAttribute("ETag")]
        public string ETag { get; set; }
    }
}
