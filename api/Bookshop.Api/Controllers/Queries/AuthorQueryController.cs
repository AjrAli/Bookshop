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
    public class AuthorQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorQueryController> _logger;
        public AuthorQueryController(IMediator mediator,
                                ILogger<AuthorQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            GetByIdQueryResponse<Author>? dataReponse = await _mediator.Send(new GetByIdQuery<Author>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllQueryResponse<Author>? dataReponse = await _mediator.Send(new GetAllQuery<Author>());
            return Ok(dataReponse);
        }
    }
}
