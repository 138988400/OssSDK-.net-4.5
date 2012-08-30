using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oss
{
    public class ObjectMetadata
    {
        private IDictionary<string, object> _metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private IDictionary<string, string> _userMetadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private const string DefaultObjectContentType = "application/octet-stream";

        internal void AddHeader(string key, object value)
        {
            this._metadata.Add(key, value);
        }

        internal void Populate(IDictionary<string, string> requestHeaders)
        {
            foreach (KeyValuePair<string, object> entry in this._metadata)
            {
                requestHeaders.Add(entry.Key, entry.Value.ToString());
            }
            if (!requestHeaders.ContainsKey("Content-Type"))
            {
                requestHeaders.Add("Content-Type", "application/octet-stream");
            }
            foreach (KeyValuePair<string, string> entry in this._userMetadata)
            {
                requestHeaders.Add("x-oss-meta-" + entry.Key, entry.Value);
            }
        }

        public string CacheControl
        {
            get
            {
                if (!this._metadata.ContainsKey("Cache-Control"))
                {
                    return null;
                }
                return (this._metadata["Cache-Control"] as string);
            }
            set
            {
                this._metadata["Cache-Control"] = value;
            }
        }

        public string ContentDisposition
        {
            get
            {
                if (!this._metadata.ContainsKey("Content-Disposition"))
                {
                    return null;
                }
                return (this._metadata["Content-Disposition"] as string);
            }
            set
            {
                this._metadata["Content-Disposition"] = value;
            }
        }

        public string ContentEncoding
        {
            get
            {
                if (!this._metadata.ContainsKey("Content-Encoding"))
                {
                    return null;
                }
                return (this._metadata["Content-Encoding"] as string);
            }
            set
            {
                this._metadata["Content-Encoding"] = value;
            }
        }

        public long ContentLength
        {
            get
            {
                if (!this._metadata.ContainsKey("Content-Length"))
                {
                    return 0;
                }
                return (long) this._metadata["Content-Length"];
            }
            internal set
            {
                this._metadata["Content-Length"] = value;
            }
        }

        public string ContentType
        {
            get
            {
                if (!this._metadata.ContainsKey("Content-Type"))
                {
                    return null;
                }
                return (this._metadata["Content-Type"] as string);
            }
            set
            {
                this._metadata["Content-Type"] = value;
            }
        }

        public string ETag
        {
            get
            {
                if (!this._metadata.ContainsKey("ETag"))
                {
                    return null;
                }
                return (this._metadata["ETag"] as string);
            }
            set
            {
                this._metadata["ETag"] = value;
            }
        }

        public DateTime ExpirationTime
        {
            get
            {
                if (!this._metadata.ContainsKey("Expires"))
                {
                    return DateTime.MinValue;
                }
                return (DateTime) this._metadata["Expires"];
            }
            internal set
            {
                this._metadata["Expires"] = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                if (!this._metadata.ContainsKey("Last-Modified"))
                {
                    return DateTime.MinValue;
                }
                return (DateTime) this._metadata["Last-Modified"];
            }
            internal set
            {
                this._metadata["Last-Modified"] = value;
            }
        }

        public IDictionary<string, string> UserMetadata
        {
            get
            {
                return this._userMetadata;
            }
        }
    }
}
