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
    class ListPartsDeserialzer : ResponseDeserializer<ListPartsResult, ListPartsResult>
    {
        public ListPartsDeserialzer(IDeserializer<Stream, ListPartsResult> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override async Task<ListPartsResult> Deserialize(HttpResponseMessage response)
        {
            ListPartsResult model = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            return model;
        }
    }

}
