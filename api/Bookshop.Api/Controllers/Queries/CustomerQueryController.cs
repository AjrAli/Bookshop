using Bookshop.Application.Features.Common.Queries.Customers;
using Bookshop.Application.Features.Customers.Queries.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/customer")]
    public class CustomerQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerQueryController> _logger;
        public CustomerQueryController(IMediator mediator,
                                ILogger<CustomerQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetCustomerById
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllCustomers());
            return Ok(dataReponse);
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Authenticate request)
        {
            var dataReponse = await _mediator.Send(request);
            return Ok(dataReponse);
        }
    }
}
