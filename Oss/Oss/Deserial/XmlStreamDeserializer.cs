using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Oss.Deserial
{
    internal class XmlStreamDeserializer<T> : IDeserializer<Stream, T>
    {
        public T Deserialize(Stream xml)
        {
            T model;
            Stream xmlStream = xml;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                model = (T)serializer.Deserialize(xml);
            }
            catch (XmlException ex)
            {
                throw new ResponseDeserializationException(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ResponseDeserializationException(ex.Message, ex);
            }
            finally
            {
                if (xmlStream != null)
                {
                    xmlStream.Dispose();
                }
            }
            return model;
        }
    }
}
