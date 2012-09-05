using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("ListPartsResult")]
    public class ListPartsResult
    {
        public ListPartsResult()
        {
            Parts = new List<ListPartModel>();
        }


        [XmlElement]
        public string Bucket { get; set; }
        [XmlElement]
        public string Key { get; set; }
        [XmlElement]
        public UInt32 NextPartNumberMarker { get; set; }
        [XmlElement]
        public UInt32 MaxParts { get; set; }
        [XmlElement]
        public bool IsTruncated { get; set; }

        [XmlElement("Part")]
        public List<ListPartModel> Parts { get; set; }
    }
}
