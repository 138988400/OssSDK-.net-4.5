using Oss.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Commands
{
    internal  class OssCommand : ICommand<bool>
    {
        public OssCommand(HttpClient _httpClient, OssHttpRequestMessage _ossHttpRequestMessage,
            NetworkCredential _networkCredential)
        {
            httpClient = _httpClient;
            ossHttpRequestMessage = _ossHttpRequestMessage;
            networkCredential = _networkCredential;
        }
        public HttpClient httpClient;
        public OssHttpRequestMessage ossHttpRequestMessage;
        public NetworkCredential networkCredential;

        public async Task<bool> Execute()
        {
            OssRequestSigner.Sign(ossHttpRequestMessage, networkCredential);
            HttpResponseMessage test = await httpClient.SendAsync(ossHttpRequestMessage);

            if (test.IsSuccessStatusCode == false)
            {
                ErrorResponseHandler handler = new ErrorResponseHandler();
                handler.Handle(test);
                return false;
            }
            return true;
        }
    }
}
