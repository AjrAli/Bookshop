using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Commands.DeleteShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;
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
        [HttpPost("update-shoppingcart")]
        public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartRequestDto shoppingCartDto)
        {
            var dataCommandReponse = await _mediator.Send(new UpdateShoppingCart
            {
                ShoppingCart = shoppingCartDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost]
        [Route("delete-shoppingcart")]
        public async Task<IActionResult> DeleteSchool([FromBody] long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteShoppingCart
            {
                ShoppingCartId = id
            });

            return Ok(dataCommandReponse);
        }
    }
}
