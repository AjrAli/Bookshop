using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.Orders.Queries.GetOrderById;
using Bookshop.Application.Features.Orders.Queries.GetOrders;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/order")]
    public class OrderQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderQueryController> _logger;
        private readonly ILoggedInUserService? _loggedInUserService;
        public OrderQueryController(IMediator mediator,
                                ILogger<OrderQueryController> logger,
                                ILoggedInUserService? loggedInUserService)
        {
            _mediator = mediator;
            _logger = logger;
            _loggedInUserService = loggedInUserService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetById<OrderResponseDto>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAll<OrderResponseDto>());
            return Ok(dataReponse);
        }
        [Authorize]
        [HttpGet("get-user-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var dataReponse = await _mediator.Send(new GetOrders
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
        [Authorize]
        [HttpGet("get-user-order/{id}")]
        public async Task<IActionResult> GetOrderById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetOrderById
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = id
            });
            return Ok(dataReponse);
        }
    }
}
