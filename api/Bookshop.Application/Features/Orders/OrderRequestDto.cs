using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders
{
    public class OrderRequestDto
    {
        public long Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
        public string? MethodOfPayment { get; set;}
    }
}
