using Bookshop.Application.Features.Customers.Queries.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Authenticate request)
        {
            var dataReponse = await _mediator.Send(request);
            return Ok(dataReponse);
        }
    }
}
