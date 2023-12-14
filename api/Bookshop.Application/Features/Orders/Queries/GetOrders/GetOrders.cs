using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrders : IQuery<GetOrdersResponse>
    {
        public string? UserId { get; set; }
    }
}
