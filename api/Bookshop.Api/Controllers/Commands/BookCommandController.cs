using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Books.Commands.Comments.AddComment;
using Bookshop.Application.Features.Books.Commands.Comments.DeleteComment;
using Bookshop.Application.Features.Books.Commands.Comments.UpdateComment;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("api/book")]
    public class BookCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService _loggedInUserService;
        public BookCommandController(IMediator mediator,
                                ILoggedInUserService loggedInUserService)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("add-comment-book")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequestDto commentDto)
        {
            commentDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new AddComment
            {
                Comment = commentDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("delete-comment-book")]
        public async Task<IActionResult> DeleteComment([FromBody] long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteComment
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = id
            });

            return Ok(dataCommandReponse);
        }
        [HttpPost("update-comment-book")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentRequestDto commentDto)
        {
            commentDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new UpdateComment
            {
                Comment = commentDto
            });
            return Ok(dataCommandReponse);
        }
    }
}
