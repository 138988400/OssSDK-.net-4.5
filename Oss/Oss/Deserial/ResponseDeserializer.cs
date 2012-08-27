using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal abstract class ResponseDeserializer<TResult, TModel> : IDeserializer<HttpResponseMessage, Task<TResult>>
    {
        public ResponseDeserializer(IDeserializer<Stream, TModel> contentDeserializer)
        {
            this.ContentDeserializer = contentDeserializer;
        }

        public IDeserializer<Stream, TModel> ContentDeserializer;
        public abstract  Task<TResult> Deserialize(HttpResponseMessage response);

    }
}
