using Bookshop.Application.Contracts.MediatR.Command;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrder : ICommand<CancelOrderResponse>
    {
        public long? Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
