using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Customers.Commands.CreateCustomer;
using Bookshop.Application.Features.Customers.Commands.EditCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    public class CustomerCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerCommandController> _logger;
        public CustomerCommandController(IMediator mediator,
                                ILogger<CustomerCommandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestDto customerDto)
        {
            var dataCommandReponse = await _mediator.Send(new CreateCustomer
            {
                Customer = customerDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("edit-customer")]
        public async Task<IActionResult> EditCustomer([FromBody] EditCustomerDto customerDto)
        {
            var dataCommandReponse = await _mediator.Send(new EditCustomer
            {
                Customer = customerDto
            });
            return Ok(dataCommandReponse);
        }
    }
}
