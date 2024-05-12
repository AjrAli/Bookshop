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
    [Route("api/shopcarts")]
    public class ShoppingCartQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService? _loggedInUserService;
        public ShoppingCartQueryController(IMediator mediator,
                                ILoggedInUserService? loggedInUserService)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
        }

        [HttpGet("current")]
        public async Task<IActionResult> RetrieveUserShoppingCartDetails()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCart
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }

        [HttpGet("current/reviews")]
        public async Task<IActionResult> GetUserShoppingCartDetailsWithReviews()
        {
            var dataReponse = await _mediator.Send(new GetShoppingCartDetails
            {
                UserId = _loggedInUserService?.GetUserId()
            });
            return Ok(dataReponse);
        }
    }
}
