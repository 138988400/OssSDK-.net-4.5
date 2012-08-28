using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("ListAllMyBucketsResult")]
    public class ListAllMyBucketsResult
    {
        [XmlArrayItem("Bucket")]
        public List<BucketModel> Buckets { get; set; }

        [XmlElement("Owner")]
        public Owner Owner { get; set; }
    }
}
