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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            GetByIdQueryResponse<Customer>? dataReponse = await _mediator.Send(new GetByIdQuery<Customer>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllQueryResponse<Customer>? dataReponse = await _mediator.Send(new GetAllQuery<Customer>());
            return Ok(dataReponse);
        }
    }
}
