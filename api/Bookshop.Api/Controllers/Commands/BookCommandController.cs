using Bookshop.Api.Utility.FileRequest;
using Bookshop.Api.Utility.Service;
using Bookshop.Application.Configuration;
using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Books.Commands.Comments.AddComment;
using Bookshop.Application.Features.Books.Commands.Comments.DeleteComment;
using Bookshop.Application.Features.Books.Commands.Comments.UpdateComment;
using Bookshop.Application.Features.Books.Commands.CreateBook;
using Bookshop.Application.Features.Books.Commands.DeleteBook;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HeyRed.Mime.MimeTypesMap;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("api/book")]
    public class BookCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _environment;
        public BookCommandController(IMediator mediator,
                                ILoggedInUserService loggedInUserService,
                                IFileService fileService,
                                IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
            _fileService = fileService;
            _environment = environment;
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
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost("create-book")]
        public async Task<IActionResult> CreateBook([FromForm] BookFileRequestDto bookFileDto)
        {
            if (bookFileDto.Book == null)
                return BadRequest("BookDto is null.");
            // Check if the uploaded file is an image with valid size
            if (!_fileService.IsFileValid(bookFileDto.File))
                return BadRequest("Invalid file type/length. Please upload an image with valid type.");

            bookFileDto.Book.Image = await _fileService.GetFileContent(bookFileDto.File);
            if (bookFileDto.Book.Image == null || bookFileDto.Book.Image.Length == 0)
                return BadRequest("Image is empty/invalid");
            bookFileDto.Book.UploadImageDirectory = Path.Combine(_environment.ContentRootPath, @"Client", "img");
            bookFileDto.Book.ImageExtension = GetExtension(bookFileDto.File.ContentType);
            var dataCommandReponse = await _mediator.Send(new CreateBook
            {
                Book = bookFileDto.Book
            });
            return Ok(dataCommandReponse);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost("delete-book")]
        public async Task<IActionResult> DeleteBook([FromBody] long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteBook
            {
                Id = id,
                UploadImageDirectory = Path.Combine(_environment.ContentRootPath, @"Client", "img")
            });
            return Ok(dataCommandReponse);
        }
    }
}
