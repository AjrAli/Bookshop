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
            GetByIdQueryResponse<Order>? dataReponse = await _mediator.Send(new GetByIdQuery<Order>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllQueryResponse<Order>? dataReponse = await _mediator.Send(new GetAllQuery<Order>());
            return Ok(dataReponse);
        }
    }
}
