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
    internal class ErrorResponseHandler : ResponseHandler
    {
        public override async void Handle(HttpResponseMessage response)
        {
            base.Handle(response);
            if (!response.IsSuccessStatusCode)
            {
                ErrorResult errorResult = null;
                try
                {
                    IDeserializer<HttpResponseMessage, Task<ErrorResult>> d = DeserializerFactory.GetFactory().CreateErrorResultDeserializer();

                    errorResult = await d.Deserialize(response);
                }
                catch (XmlException)
                {
                    //response.EnsureSuccessful();
                }
                catch (InvalidOperationException)
                {
                    //response.EnsureSuccessful();
                }
                throw ExceptionFactory.CreateException(errorResult.Code, errorResult.Message, errorResult.RequestId, errorResult.HostId);
            }
        }
    }
}
