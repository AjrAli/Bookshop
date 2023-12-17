using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrders : IQuery<GetOrdersResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
