using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("CompleteMultipartUpload")]
    public class CompleteMultipartUploadModel
    {
        public CompleteMultipartUploadModel()
        {
            Parts = new List<MultipartUploadPartModel>();
        }
        [XmlElement("Part")]
        public List<MultipartUploadPartModel> Parts { get; set; }

        [XmlIgnoreAttribute]
        public string Bucket { get; set; }
        [XmlIgnoreAttribute]
        public string Key { get; set; }
        [XmlIgnoreAttribute]
        public string UploadId { get; set; }
    }
}
