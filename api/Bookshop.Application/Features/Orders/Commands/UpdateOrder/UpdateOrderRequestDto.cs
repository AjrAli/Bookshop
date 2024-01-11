using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequestDto
    {
        public long Id { get; set; }
        public IList<long>? ItemsId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
