using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Queries.GetOrdersOfCustomer
{
    public class GetOrdersOfCustomer : IQuery<GetOrdersOfCustomerResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
