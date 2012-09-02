using Oss.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal class ListObjectsResponseDeserializer : ResponseDeserializer<ObjectListing, ListBucketResult>
    {
        public ListObjectsResponseDeserializer(IDeserializer<Stream, ListBucketResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public async override Task<ObjectListing> Deserialize(HttpResponseMessage response)
        {
            ListBucketResult listBucketResult = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            ObjectListing objectList = new ObjectListing(listBucketResult.Name)
            {
                Delimiter = listBucketResult.Delimiter,
                IsTrunked = listBucketResult.IsTruncated,
                Marker = listBucketResult.Marker,
                MaxKeys = listBucketResult.MaxKeys,
                NextMarker = listBucketResult.NextMarker,
                Prefix = listBucketResult.Prefix
            };
            if (listBucketResult.Contents != null)
            {
                foreach (ListBucketResultContents contents in listBucketResult.Contents)
                {
                    OssObjectSummary summary = new OssObjectSummary
                    {
                        BucketName = listBucketResult.Name,
                        Key = contents.Key,
                        LastModified = contents.LastModified,
                        ETag = (contents.ETag != null) ? contents.ETag.Trim(new char[] { '"' }) : string.Empty,
                        Size = contents.Size,
                        StorageClass = contents.StorageClass,
                        Owner = (contents.Owner != null) ? new Owner(contents.Owner.Id, contents.Owner.DisplayName) : null
                    };
                    objectList.AddObjectSummary(summary);
                }
            }
            if (listBucketResult.CommonPrefixes != null)
            {
                foreach (ListBucketResultCommonPrefixes commonPrefixes in listBucketResult.CommonPrefixes)
                {
                    if (commonPrefixes.Prefix != null)
                    {
                        foreach (string prefix in commonPrefixes.Prefix)
                        {
                            objectList.AddCommonPrefix(prefix);
                        }
                    }
                }
            }
            return objectList;
        }
    }
}
