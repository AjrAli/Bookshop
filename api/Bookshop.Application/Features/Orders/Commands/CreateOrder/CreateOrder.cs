using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrder : ICommand<CreateOrderResponse>
    {
        public OrderRequestDto? Order { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
