using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.Orders.Commands.CancelOrder;
using Bookshop.Application.Features.Orders.Commands.CreateOrder;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("order")]
    public class OrderCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderCommandController> _logger;
        private readonly ILoggedInUserService _loggedInUserService;
        public OrderCommandController(IMediator mediator,
                                ILogger<OrderCommandController> logger,
                                ILoggedInUserService loggedInUserService)
        {
            _mediator = mediator;
            _logger = logger;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("create-user-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto orderDto)
        {
            orderDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new CreateOrder
            {
                Order = orderDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("cancel-user-order/{id}")]
        public async Task<IActionResult> CancelOrder(long? id)
        {
            var dataCommandReponse = await _mediator.Send(new CancelOrder
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = id
            });

            return Ok(dataCommandReponse);
        }
    }
}
