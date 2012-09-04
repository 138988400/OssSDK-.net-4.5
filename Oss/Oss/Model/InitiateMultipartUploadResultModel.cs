using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{

    [XmlRoot("InitiateMultipartUploadResult")]
    public class InitiateMultipartUploadResultModel
    {
        [XmlElement("Bucket")]
        public string Bucket { get; set; }

        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("UploadId")]
        public string UploadId { get; set; }
    }
}
