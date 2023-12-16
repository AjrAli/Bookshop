using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdResponse : BaseResponse
    {
        public object? Dto { get; set; }
        public GetByIdResponse() : base()
        {

        }
    }
}
