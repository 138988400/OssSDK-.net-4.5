using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("CompleteMultipartUploadResult")]
    public class CompleteMultipartUploadResult
    {
        [XmlElement("Location")]
        public string Location { get; set; }

        [XmlElement("Bucket")]
        public string Bucket { get; set; }

        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("ETag")]
        public string ETag { get; set; }
    }
}
