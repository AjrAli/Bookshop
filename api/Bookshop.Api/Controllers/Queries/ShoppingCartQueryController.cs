using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            var dataReponse = await _mediator.Send(new GetByIdQuery<ShoppingCart>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllQuery<ShoppingCart>());
            return Ok(dataReponse);
        }
    }
}
