using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Authorize]
    [Route("api/shopcart")]
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

        [HttpGet("get-user-shopcart")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCart
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }

        [HttpGet("get-user-shopcart-details-review")]
        public async Task<IActionResult> GetShoppingCartDetails()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCartDetails
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
    }
}
