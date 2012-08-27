using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal class XmlDeserializerFactory : DeserializerFactory
    {
        protected override IDeserializer<Stream, T> CreateContentDeserializer<T>()
        {
            return new XmlStreamDeserializer<T>();
        }
    }
}
