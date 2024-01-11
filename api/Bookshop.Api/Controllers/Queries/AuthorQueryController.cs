using Bookshop.Application.Features.Common.Queries.Authors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Route("api/author")]
    public class AuthorQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetAuthorById
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllAuthors());
            return Ok(dataReponse);
        }
    }
}
