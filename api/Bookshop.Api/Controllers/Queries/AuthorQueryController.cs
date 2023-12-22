using Bookshop.Application.Features.Authors;
using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/author")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetById<AuthorResponseDto>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAll<AuthorResponseDto>());
            return Ok(dataReponse);
        }
    }
}
