using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Oss
{
    class XmlStreamSerializer<T>
    {
       public  Stream Serialize(T model)
        {
            MemoryStream stream = new MemoryStream();
            
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, model);

            return stream;
        }
    }
}
