using Bookshop.Application.Contracts.MediatR.Query;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetails : IQuery<GetShoppingCartDetailsResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
