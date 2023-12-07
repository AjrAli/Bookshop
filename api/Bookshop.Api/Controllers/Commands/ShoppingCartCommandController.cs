using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    public class ShoppingCartCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShoppingCartCommandController> _logger;
        public ShoppingCartCommandController(IMediator mediator,
                                ILogger<ShoppingCartCommandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create-shoppingcart")]
        public async Task<IActionResult> CreateShoppingCart([FromBody] ShoppingCartRequestDto shoppingCartDto)
        {
            var dataCommandReponse = await _mediator.Send(new CreateShoppingCart
            {
                ShoppingCart = shoppingCartDto
            });
            return Ok(dataCommandReponse);
        }
    }
}
