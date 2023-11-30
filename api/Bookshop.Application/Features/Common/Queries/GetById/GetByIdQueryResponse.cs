using Bookshop.Application.Features.Category;
using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdQueryResponse<T> : BaseResponse where T : class
    {
        public T? Dto { get; set; }
        public GetByIdQueryResponse() : base()
        {

        }
    }
}
