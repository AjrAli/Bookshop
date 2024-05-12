using Bookshop.Application.Configuration;
using Bookshop.Application.Features.Orders.Queries;
using Bookshop.Application.Features.Orders.Queries.GetOrderByIdOfCustomer;
using Bookshop.Application.Features.Orders.Queries.GetOrdersOfCustomer;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Authorize]
    public class OrderQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService? _loggedInUserService;
        public OrderQueryController(IMediator mediator,
                                ILoggedInUserService? loggedInUserService)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
        }
        [HttpGet("api/orders")]
        public async Task<IActionResult> GetAllUserOrders()
        {
            var dataReponse = await _mediator.Send(new GetOrdersOfCustomer
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
        [HttpGet("api/orders/{id}")]
        public async Task<IActionResult> GetUserOrderById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetOrderByIdOfCustomer
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = id
            });
            return Ok(dataReponse);
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpGet("api/admin/orders/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetOrderById
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpGet("api/admin/orders")]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllOrders());
            return Ok(dataReponse);
        }
    }
}
