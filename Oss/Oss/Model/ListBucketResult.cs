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
    [Serializable, XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true), GeneratedCode("xsd", "4.0.30319.1"), DebuggerStepThrough, DesignerCategory("code")]
    public class ListBucketResult
    {
        private ListBucketResultCommonPrefixes[] commonPrefixesField;
        private ListBucketResultContents[] contentsField;
        private string delimiterField;
        private bool isTruncatedField;
        private string markerField;
        private int maxKeysField;
        private string nameField;
        private string nextMarkerField;
        private string prefixField;

        [XmlElement("CommonPrefixes")]
        public ListBucketResultCommonPrefixes[] CommonPrefixes
        {
            get
            {
                return this.commonPrefixesField;
            }
            set
            {
                this.commonPrefixesField = value;
            }
        }

        [XmlElement("Contents")]
        public ListBucketResultContents[] Contents
        {
            get
            {
                return this.contentsField;
            }
            set
            {
                this.contentsField = value;
            }
        }

        public string Delimiter
        {
            get
            {
                return this.delimiterField;
            }
            set
            {
                this.delimiterField = value;
            }
        }

        public bool IsTruncated
        {
            get
            {
                return this.isTruncatedField;
            }
            set
            {
                this.isTruncatedField = value;
            }
        }

        public string Marker
        {
            get
            {
                return this.markerField;
            }
            set
            {
                this.markerField = value;
            }
        }

        public int MaxKeys
        {
            get
            {
                return this.maxKeysField;
            }
            set
            {
                this.maxKeysField = value;
            }
        }

        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public string NextMarker
        {
            get
            {
                return this.nextMarkerField;
            }
            set
            {
                this.nextMarkerField = value;
            }
        }

        public string Prefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }
    }
}
