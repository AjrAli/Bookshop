using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdResponse<T> : BaseResponse where T : class
    {
        public T? Dto { get; set; }
        public GetByIdResponse() : base()
        {

        }
    }
}
