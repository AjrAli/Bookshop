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
    public class CustomerQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerQueryController> _logger;
        public CustomerQueryController(IMediator mediator,
                                ILogger<CustomerQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildCustomerQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetByIdQuery<Customer>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildCustomerQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAllQuery<Customer>()
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<Customer, object>>, List<Expression<Func<object, object>>>> BuildCustomerQueryConfiguration()
        {
            return new Dictionary<Expression<Func<Customer, object>>, List<Expression<Func<object, object>>>>
            {
                { x => x.ShippingAddress, null },
                { x => x.BillingAddress, null },
                { x => x.ShoppingCart, new List<Expression<Func<object, object>>>
                    {
                        y => (y as ShoppingCart).LineItems,
                        z => (z as LineItem).Book,
                        w => (w as Book).Author,
                    }
                }
            };
        }
    }
}
