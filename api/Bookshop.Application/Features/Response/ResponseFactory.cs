using Bookshop.Application.Features.Response.Contracts;

namespace Bookshop.Application.Features.Response
{
    public class ResponseFactory<T> : IResponseFactory<T> where T : BaseResponse, new()
    {
        public T CreateResponse()
        {
            return new T();
        }
    }
}
