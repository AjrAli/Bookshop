using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class OrderQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderQueryController> _logger;
        public OrderQueryController(IMediator mediator,
                                ILogger<OrderQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildOrderQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetByIdQuery<Order>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildOrderQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAllQuery<Order>
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<Order, object>>, List<Expression<Func<object, object>>>> BuildOrderQueryConfiguration()
        {
            return new Dictionary<Expression<Func<Order, object>>, List<Expression<Func<object, object>>>>
            {
                { x => x.LineItems, new List<Expression<Func<object, object>>>
                    {
                        y => (y as LineItem).Book
                    }
                },
                { x => x.Customer, null }
            };
        }
    }
}
