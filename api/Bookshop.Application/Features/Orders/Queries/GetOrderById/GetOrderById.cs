using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderById : IQuery<GetOrderByIdResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public long? Id { get; set; }
    }
}
