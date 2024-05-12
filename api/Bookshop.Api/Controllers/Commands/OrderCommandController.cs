using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.Orders.Commands.CancelOrder;
using Bookshop.Application.Features.Orders.Commands.CreateOrder;
using Bookshop.Application.Features.Orders.Commands.UpdateOrder;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("api/orders")]
    public class OrderCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService _loggedInUserService;
        public OrderCommandController(IMediator mediator,
                                ILoggedInUserService loggedInUserService)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto orderDto)
        {
            orderDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new CreateOrder
            {
                Order = orderDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(long? id)
        {
            var dataCommandReponse = await _mediator.Send(new CancelOrder
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = id
            });

            return Ok(dataCommandReponse);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequestDto orderDto, long id)
        {
            orderDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new UpdateOrder
            {
                Order = orderDto,
                Id = id
            });
            return Ok(dataCommandReponse);
        }
    }
}
