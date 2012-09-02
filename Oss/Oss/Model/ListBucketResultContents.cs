using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Oss.Model
{
    [Serializable, XmlType(AnonymousType = true), DebuggerStepThrough, GeneratedCode("xsd", "4.0.30319.1"), DesignerCategory("code")]
    public class ListBucketResultContents
    {
        private string eTagField;
        private string keyField;
        private DateTime lastModifiedField;
        private Owner ownerField;
        private long sizeField;
        private string storageClassField;
        private string typeField;

        public string ETag
        {
            get
            {
                return this.eTagField;
            }
            set
            {
                this.eTagField = value;
            }
        }

        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return this.lastModifiedField;
            }
            set
            {
                this.lastModifiedField = value;
            }
        }

        public Owner Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }

        public long Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        public string StorageClass
        {
            get
            {
                return this.storageClassField;
            }
            set
            {
                this.storageClassField = value;
            }
        }

        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
}
