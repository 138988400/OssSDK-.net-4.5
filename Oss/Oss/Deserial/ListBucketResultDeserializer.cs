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
    internal class ListBucketResultDeserializer : ResponseDeserializer<IEnumerable<Bucket>, ListAllMyBucketsResult>
    {
        public ListBucketResultDeserializer(IDeserializer<Stream, ListAllMyBucketsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override async Task<IEnumerable<Bucket>> Deserialize(HttpResponseMessage response)
        {
            ListAllMyBucketsResult model = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            return (from e in model.Buckets select new Bucket(e.Name) { Owner = new Owner(model.Owner.Id, model.Owner.DisplayName), CreationDate = e.CreationDate });
        }
    }
}
