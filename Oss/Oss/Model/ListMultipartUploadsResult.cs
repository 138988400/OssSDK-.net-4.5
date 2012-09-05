using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
     [XmlRoot("ListMultipartUploadsResult")]
    public class ListMultipartUploadsResult
    {
         public ListMultipartUploadsResult()
         {
             Uploads = new List<ListMultiPartUploadModle>();
         }
         [XmlElement]
         public string Bucket { get; set; }
         [XmlElement]
         public UInt32 MaxUploads { get; set; }
         [XmlElement]
         public bool IsTruncated { get; set; }

         [XmlElement("Upload")]
         public List<ListMultiPartUploadModle> Uploads { get; set; }
    }

}
