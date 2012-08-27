using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("AccessControlPolicy")]
    public class AccessControlPolicy
    {
        [XmlArray("AccessControlList"), XmlArrayItem("Grant")]
        public List<string> Grants { get; set; }

        [XmlElement("Owner")]
        public Owner Owner { get; set; }
    }
}
