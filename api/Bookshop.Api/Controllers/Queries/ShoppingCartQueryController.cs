using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails;
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
    [Route("[controller]")]
    public class ShoppingCartQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShoppingCartQueryController> _logger;
        private readonly ILoggedInUserService? _loggedInUserService;
        public ShoppingCartQueryController(IMediator mediator,
                                ILogger<ShoppingCartQueryController> logger,
                                ILoggedInUserService? loggedInUserService)
        {
            _mediator = mediator;
            _logger = logger;
            _loggedInUserService = loggedInUserService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildShoppingCartQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetById<ShoppingCart>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildShoppingCartQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAll<ShoppingCart>
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [Authorize]
        [HttpGet]
        [Route("get-shoppingcart")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCart
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
        [Authorize]
        [HttpGet]
        [Route("get-shoppingcart-details")]
        public async Task<IActionResult> GetShoppingCartDetails()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCartDetails
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<ShoppingCart, object>>, List<Expression<Func<object, object>>>> BuildShoppingCartQueryConfiguration()
        {
            return new Dictionary<Expression<Func<ShoppingCart, object>>, List<Expression<Func<object, object>>>>
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
