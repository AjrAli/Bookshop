﻿using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Commands.ResetShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("api/shopcarts")]
    public class ShoppingCartCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService _loggedInUserService;
        public ShoppingCartCommandController(IMediator mediator,
                                ILoggedInUserService loggedInUserService)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateShoppingCart([FromBody] ShoppingCartRequestDto shoppingCartDto)
        {
            shoppingCartDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new CreateShoppingCart
            {
                ShoppingCart = shoppingCartDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPut("")]
        public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartRequestDto shoppingCartDto)
        {
            shoppingCartDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new UpdateShoppingCart
            {
                ShoppingCart = shoppingCartDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPut("reset")]
        public async Task<IActionResult> ResetShoppingCart()
        {
            var dataCommandReponse = await _mediator.Send(new ResetShoppingCart
            {
                UserId = _loggedInUserService?.GetUserId()
            });

            return Ok(dataCommandReponse);
        }
    }
}
