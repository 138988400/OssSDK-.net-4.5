using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("Bucket")]
    public class BucketModel
    {
        [XmlElement("CreationDate")]
        public DateTime CreationDate { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
