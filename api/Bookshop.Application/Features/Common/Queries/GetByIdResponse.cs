using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries
{
    public class GetByIdResponse : BaseResponse
    {
        public object? Dto { get; set; }
        public GetByIdResponse() : base()
        {

        }
    }
}
