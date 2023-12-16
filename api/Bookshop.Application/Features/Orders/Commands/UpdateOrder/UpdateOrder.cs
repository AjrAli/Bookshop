using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrder : ICommand<UpdateOrderResponse>
    {
        public UpdateOrderRequestDto? Order { get; set; }
    }
}
