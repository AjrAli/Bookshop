using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.Orders.Queries.GetOrderById;
using Bookshop.Application.Features.Orders.Queries.GetOrders;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("order")]
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
            var queryConfig = BuildOrderQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetById<Order>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig,
                DtoType = typeof(OrderResponseDto)
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildOrderQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAll<Order>
            {
                NavigationPropertyConfigurations = queryConfig,
                DtoType = typeof(OrderResponseDto)
            });
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
        private Dictionary<Expression<Func<Order, object>>, List<Expression<Func<object, object>>>> BuildOrderQueryConfiguration()
        {
            return new()
            {
                { x => x.LineItems, new List<Expression<Func<object, object>>>
                    {
                        y => (y as LineItem).Book,
                        z => (z as Book).Author
                    }
                },
                { x => x.LineItems, new List<Expression<Func<object, object>>>
                    {
                        y => (y as LineItem).Book,
                        z => (z as Book).Category
                    }
                },
                { x => x.Customer, new List<Expression<Func<object, object>>>
                    {
                        y => (y as Customer).ShippingAddress,
                        z => (z as Address).LocationPricing
                    } 
                }
            };
        }
    }
}
