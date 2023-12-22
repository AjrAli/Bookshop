using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.Orders
{
    public class GetOrderById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
