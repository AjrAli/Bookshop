using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllResponse : BaseResponse
    {
        public List<object>? ListDto { get; set; }
        public GetAllResponse() : base()
        {

        }
    }
}
