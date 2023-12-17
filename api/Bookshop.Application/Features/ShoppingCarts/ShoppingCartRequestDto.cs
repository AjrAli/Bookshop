using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartRequestDto
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public IList<ShopItemRequestDto>? Items { get; set; }
    }
}
