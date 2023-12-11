using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Orders
{
    public class OrderRequestDto : IBaseDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
    }
}
