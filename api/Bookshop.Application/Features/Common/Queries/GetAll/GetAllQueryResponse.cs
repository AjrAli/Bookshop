using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllQueryResponse<T> : BaseResponse where T : class
    {
        public List<T>? ListDto { get; set; }
        public GetAllQueryResponse() : base()
        {

        }
    }
}
