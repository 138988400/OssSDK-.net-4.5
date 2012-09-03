using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class GetObjectRequest
    {
        private IList<string> _matchingETagConstraints = new List<string>();
        private IList<string> _nonmatchingEtagConstraints = new List<string>();
        private ResponseHeaderOverrides _responseHeaders = new ResponseHeaderOverrides();

        public GetObjectRequest(string bucketName, string key)
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            }
            if (!OssUtils.IsBucketNameValid(bucketName))
            {
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            }
            if (!OssUtils.IsObjectKeyValid(key))
            {
                throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
            }
            this.BucketName = bucketName;
            this.Key = key;
        }

        private static string JoinETag(IEnumerable<string> etags)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (string etag in etags)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                result.Append(etag);
                first = false;
            }
            return result.ToString();
        }

        internal void Populate(HttpRequestHeaders headers)
        {
            if (this.Range != null)
            {
                headers.Range = new RangeHeaderValue(this.Range[0], this.Range[1]);
            }
            if (this.ModifiedSinceConstraint.HasValue)
            {
                headers.IfModifiedSince = new DateTimeOffset(this.ModifiedSinceConstraint.Value);
            }
            if (this.UnmodifiedSinceConstraint.HasValue)
            {
                headers.IfUnmodifiedSince = new DateTimeOffset(this.UnmodifiedSinceConstraint.Value);
            }
            if (this._matchingETagConstraints.Count > 0)
            {               
                foreach (string temp in this._matchingETagConstraints)
                    headers.IfMatch.Add(new EntityTagHeaderValue(temp));
            }
            if (this._nonmatchingEtagConstraints.Count > 0)
            {
                foreach (string temp in this._nonmatchingEtagConstraints)
                    headers.IfMatch.Add(new EntityTagHeaderValue(temp));
            }
        }

        public void SetRange(long start, long end)
        {
            this.Range = new long[] { start, end };
        }

        public string BucketName { get; private set; }

        public string Key { get; private set; }

        public IList<string> MatchingETagConstraints
        {
            get
            {
                return this._matchingETagConstraints;
            }
        }

        public DateTime? ModifiedSinceConstraint { get; set; }

        public IList<string> NonmatchingETagConstraints
        {
            get
            {
                return this._nonmatchingEtagConstraints;
            }
        }

        public long[] Range { get; private set; }

        public ResponseHeaderOverrides ResponseHeaders
        {
            get
            {
                return this._responseHeaders;
            }
        }

        public DateTime? UnmodifiedSinceConstraint { get; set; }
    }
}
