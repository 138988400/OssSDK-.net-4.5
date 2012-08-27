using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Model
{
    internal class ResponseHandler : IResponseHandler
    {
        public virtual void Handle(HttpResponseMessage response)
        {
        }
    }
}
