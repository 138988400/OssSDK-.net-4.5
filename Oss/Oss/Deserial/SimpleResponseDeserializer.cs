using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal class SimpleResponseDeserializer<T> : ResponseDeserializer<T, T>
    {
        public SimpleResponseDeserializer(IDeserializer<Stream, T> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        public override async Task<T> Deserialize(HttpResponseMessage response)
        {
            using (response.Content)
            {
                return base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            }
        }
    }
}
