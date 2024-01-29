using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Orders.Queries
{
    public class GetOrderById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
