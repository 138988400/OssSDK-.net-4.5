using Oss.Deserial;
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
    internal class OssCommand<T> : ICommand<T>
    {
        private IDeserializer<HttpResponseMessage, T> _deserializer;

        public OssCommand(HttpClient _httpClient, OssHttpRequestMessage _ossHttpRequestMessage, NetworkCredential _networkCredential,
            IDeserializer<HttpResponseMessage, T> deserializer)
        {
            httpClient = _httpClient;
            ossHttpRequestMessage = _ossHttpRequestMessage;
            networkCredential = _networkCredential;
            this._deserializer = deserializer;
        }

        public HttpClient httpClient;
        public OssHttpRequestMessage ossHttpRequestMessage;
        public NetworkCredential networkCredential;


        public async Task<T> Execute()
        {

            T result;

            OssRequestSigner.Sign(ossHttpRequestMessage, networkCredential);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(ossHttpRequestMessage);

            if (responseMessage.IsSuccessStatusCode == false)
            {
                ErrorResponseHandler handler = new ErrorResponseHandler();
                handler.Handle(responseMessage);
            }

            try
            {
                result = this._deserializer.Deserialize(responseMessage);
            }
            catch (ResponseDeserializationException ex)
            {
                throw ExceptionFactory.CreateInvalidResponseException(ex);
            }

            return result;
        }

    }
}
