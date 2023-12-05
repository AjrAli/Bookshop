﻿using Bookshop.Application.Features.Customer;
using Bookshop.Application.Features.Customer.Commands;
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
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var dataCommandReponse = await _mediator.Send(new CreateCustomerCommand
            {
                Customer = customerDto
            });
            return Ok(dataCommandReponse);
        }
    }
}