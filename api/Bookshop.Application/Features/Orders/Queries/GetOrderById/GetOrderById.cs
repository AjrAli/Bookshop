using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderById : IQuery<GetOrderByIdResponse>
    {
        public string? UserId { get; set; }
        public long? Id { get; set; }
    }
}
