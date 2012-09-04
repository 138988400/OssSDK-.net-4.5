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
    [XmlRoot("CompleteMultipartUpload", Namespace = "", IsNullable = false), XmlType(AnonymousType = true), GeneratedCode("xsd", "4.0.30319.1"), DebuggerStepThrough, DesignerCategory("code")]
    public class CompleteMultipartUploadModel
    {
        [XmlElement("Part")]
        public MultipartUploadPartModel[] Parts { get; set; }
    }
}
