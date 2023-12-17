using Bookshop.Application.Contracts.MediatR.Command;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.ResetShoppingCart
{
    public class ResetShoppingCart : ICommand<ResetShoppingCartResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
