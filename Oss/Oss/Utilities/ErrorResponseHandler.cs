using Oss.Deserial;
using Oss.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Oss.Utilities
{
    internal  class ErrorResponseHandler 
    {
        static public async Task Handle(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                ErrorResult errorResult = null;
                try
                {
                    IDeserializer<HttpResponseMessage, Task<ErrorResult>> d = DeserializerFactory.GetFactory().CreateErrorResultDeserializer();

                    errorResult = await d.Deserialize(response);
                }
                catch (XmlException ex)
                {
                    throw ex;
                    //response.EnsureSuccessful();
                }
                catch (InvalidOperationException invalidEx)
                {
                    throw invalidEx;
                    //response.EnsureSuccessful();
                }
                throw ExceptionFactory.CreateException(errorResult.Code, errorResult.Message, errorResult.RequestId, errorResult.HostId);
            }
        }
    }
}
