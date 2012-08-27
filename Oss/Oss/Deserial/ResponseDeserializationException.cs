using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    [Serializable]
    internal class ResponseDeserializationException : InvalidOperationException, ISerializable
    {
        public ResponseDeserializationException()
        {
        }

        public ResponseDeserializationException(string message)
            : base(message)
        {
        }

        protected ResponseDeserializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ResponseDeserializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
