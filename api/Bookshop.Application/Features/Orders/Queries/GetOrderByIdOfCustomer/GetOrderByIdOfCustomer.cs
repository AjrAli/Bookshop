using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderByIdOfCustomer
{
    public class GetOrderByIdOfCustomer : IQuery<GetOrderByIdOfCustomerResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public long? Id { get; set; }
    }
}
