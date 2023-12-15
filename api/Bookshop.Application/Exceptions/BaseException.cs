using Bookshop.Application.Features.Response;
using System.Runtime.Serialization;

namespace Bookshop.Application.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public ErrorResponse CreateErrorResponse(string? newValidationErrorStr = null)
        {
            if (newValidationErrorStr != null)
            {
                return new(newValidationErrorStr);
            }
            return new(Message);
        }
    }
}
