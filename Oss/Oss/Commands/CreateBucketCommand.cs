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
    class CreateBucketCommand : ICommand<bool>
    {
        private CreateBucketCommand(HttpClient _httpClient, string bucketName, NetworkCredential _networkCredential)
        {
            httpClient = _httpClient;

            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException(OssResources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            }

            if (!OssUtils.IsBucketNameValid(bucketName))
            {
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
            }


            ossHttpRequestMessage = new OssHttpRequestMessage(bucketName, null);

            ossHttpRequestMessage.Method = HttpMethod.Put;
            ossHttpRequestMessage.Headers.Date = DateTime.UtcNow;
            networkCredential = _networkCredential;

        }

        public HttpClient httpClient;
        public OssHttpRequestMessage ossHttpRequestMessage;
        public NetworkCredential networkCredential;


        public static CreateBucketCommand Create(HttpClient httpClient, string bucketName, NetworkCredential _networkCredential)
        {
            return new CreateBucketCommand(httpClient, bucketName, _networkCredential);
        }

        public async Task<bool> Execute()
        {
            OssCommand ossCommand = new OssCommand(httpClient, ossHttpRequestMessage, networkCredential);
            return await ossCommand.Execute();
        }

    }
}
