﻿using Bookshop.Application.Features.Common.Queries.GetAll;
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
    public class ShoppingCartQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShoppingCartQueryController> _logger;
        public ShoppingCartQueryController(IMediator mediator,
                                ILogger<ShoppingCartQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
