using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequestDto
    {
        public IList<long>? ItemsId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
