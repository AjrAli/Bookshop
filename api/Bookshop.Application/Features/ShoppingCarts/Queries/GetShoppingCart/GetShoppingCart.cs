using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart
{
    public class GetShoppingCart : IQuery<GetShoppingCartResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
