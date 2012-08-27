using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Utilities
{
    internal static class ExceptionFactory
    {
        public static OssException CreateException(string errorCode, string message, string requestId, string hostId)
        {
            return CreateException(errorCode, message, requestId, hostId, null);
        }

        public static OssException CreateException(string errorCode, string message, string requestId, string hostId, Exception innerException)
        {
            OssException exception = (innerException != null) ? new OssException(message, innerException) : new OssException(message);
            exception.RequestId = requestId;
            exception.HostId = hostId;
            exception.ErrorCode = errorCode;
            return exception;
        }

        public static Exception CreateInvalidResponseException(Exception innerException)
        {
            throw new InvalidOperationException(OssResources.ExceptionInvalidResponse, innerException);
        }
    }
}
