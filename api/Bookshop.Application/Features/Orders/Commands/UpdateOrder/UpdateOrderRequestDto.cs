using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequestDto : IBaseDto
    {
        public long Id { get; set; }
        public IList<long>? ItemsId { get; set; }
        public string? UserId { get; set; }
    }
}
