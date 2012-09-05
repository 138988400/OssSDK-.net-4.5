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
    class ListMultipartUploadsDeserializer : ResponseDeserializer<ListMultipartUploadsResult, ListMultipartUploadsResult>
    {
        public ListMultipartUploadsDeserializer(IDeserializer<Stream, ListMultipartUploadsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override async Task<ListMultipartUploadsResult> Deserialize(HttpResponseMessage response)
        {
            ListMultipartUploadsResult model = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            return model;
        }
    }

}
