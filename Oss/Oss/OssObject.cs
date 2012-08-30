using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class OssObject : IDisposable
    {
        private bool _disposed;
        private string _key;
        private ObjectMetadata _metadata = new ObjectMetadata();

        public OssObject(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            }
            this._key = key;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (this.Content != null)
                {
                    this.Content.Dispose();
                }
                this._disposed = true;
            }
        }

        public override string ToString()
        {
            object[] temp = new object[] { this._key, this.BucketName ?? string.Empty };
            return string.Format(CultureInfo.InvariantCulture, "[OSSObject Key={0}, BucketName={1}]", temp);
        }

        public string BucketName { get; set; }

        public Stream Content { get; set; }

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "value");
                }
                this._key = value;
            }
        }

        public ObjectMetadata Metadata
        {
            get
            {
                return this._metadata;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._metadata = value;
            }
        }
    }
}
