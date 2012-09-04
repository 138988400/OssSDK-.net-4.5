using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
     [XmlRoot("CompleteMultipartUpload")]
    public class CompleteMultipartUploadModel
    {
         [XmlElement("Part")]
         public MultipartUploadPartModel[] Parts { get; set; }
    }
}
