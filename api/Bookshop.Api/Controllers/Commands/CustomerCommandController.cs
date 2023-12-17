using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Customers.Commands.CreateCustomer;
using Bookshop.Application.Features.Customers.Commands.EditCustomer;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerCommandController> _logger;
        private readonly ILoggedInUserService? _loggedInUserService;
        public CustomerCommandController(IMediator mediator,
                                ILogger<CustomerCommandController> logger,
                                 ILoggedInUserService? loggedInUserService)
        {
            _mediator = mediator;
            _logger = logger;
            _loggedInUserService = loggedInUserService;
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
        [Authorize]
        [HttpPost("edit-user-customer")]
        public async Task<IActionResult> EditCustomer([FromBody] EditCustomerDto customerDto)
        {
            customerDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new EditCustomer
            {
                Customer = customerDto
            });
            return Ok(dataCommandReponse);
        }
    }
}
