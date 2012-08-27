using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss.Model
{
    [XmlRoot("Error")]
    public class ErrorResult
    {
        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("HostID")]
        public string HostId { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("RequestID")]
        public string RequestId { get; set; }
    }
}
