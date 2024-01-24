using Bookshop.Application.Configuration;
using Bookshop.Application.Features.Authors;
using Bookshop.Application.Features.Authors.Commands.CreateAuthor;
using Bookshop.Application.Features.Authors.Commands.DeleteAuthor;
using Bookshop.Application.Features.Authors.Commands.UpdateAuthor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize(Roles = RoleNames.Administrator)]
    [Route("api/author")]
    public class AuthorCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-author")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequestDto authorDto)
        {
            var dataCommandReponse = await _mediator.Send(new CreateAuthor
            {
                Author = authorDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("update-author")]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorRequestDto authorDto)
        {
            var dataCommandReponse = await _mediator.Send(new UpdateAuthor
            {
                Author = authorDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("delete-author")]
        public async Task<IActionResult> DeleteAuthor([FromBody] long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteAuthor
            {
                Id = id
            });
            return Ok(dataCommandReponse);
        }
    }
}
