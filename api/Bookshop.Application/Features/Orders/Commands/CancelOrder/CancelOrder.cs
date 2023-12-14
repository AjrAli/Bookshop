using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrder : ICommand<CancelOrderResponse>
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
    }
}
