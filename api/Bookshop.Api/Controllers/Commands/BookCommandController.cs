using Bookshop.Api.Utility.FileRequest;
using Bookshop.Api.Utility.Service;
using Bookshop.Application.Configuration;
using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Books.Commands.Comments.AddComment;
using Bookshop.Application.Features.Books.Commands.Comments.DeleteComment;
using Bookshop.Application.Features.Books.Commands.Comments.UpdateComment;
using Bookshop.Application.Features.Books.Commands.CreateBook;
using Bookshop.Application.Features.Books.Commands.DeleteBook;
using Bookshop.Application.Features.Books.Commands.UpdateBook;
using Bookshop.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HeyRed.Mime.MimeTypesMap;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize]
    [Route("api/books")]
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

        [HttpPost("{bookId}/comments")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequestDto commentDto, long bookId)
        {
            commentDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new AddComment
            {
                Comment = commentDto,
                BookId = bookId
            });
            return Ok(dataCommandReponse);
        }
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteComment
            {
                UserId = _loggedInUserService?.GetUserId(),
                Id = commentId
            });

            return Ok(dataCommandReponse);
        }
        [HttpPut("comments/{commentId}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentRequestDto commentDto, long commentId)
        {
            commentDto.UserId = _loggedInUserService?.GetUserId();
            var dataCommandReponse = await _mediator.Send(new UpdateComment
            {
                Comment = commentDto,
                Id = commentId
                
            });
            return Ok(dataCommandReponse);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost("")]
        public async Task<IActionResult> CreateBook([FromForm] BookFileRequestDto bookFileDto)
        {
            var resultValidation = await ValidateRequest(bookFileDto);
            if (resultValidation != null)
                return resultValidation;

            await AssignImage(bookFileDto);
            var dataCommandReponse = await _mediator.Send(new CreateBook
            {
                Book = bookFileDto.Book
            });
            return Ok(dataCommandReponse);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromForm] BookFileRequestDto bookFileDto, long id)
        {
            var resultValidation = await ValidateRequest(bookFileDto);
            if (resultValidation != null)
                return resultValidation;

            await AssignImage(bookFileDto);
            var dataCommandReponse = await _mediator.Send(new UpdateBook
            {
                Book = bookFileDto.Book,
                Id = id
            });
            return Ok(dataCommandReponse);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteBook
            {
                Id = id,
                UploadImageDirectory = Path.Combine(_environment.ContentRootPath, @"Client", "img")
            });
            return Ok(dataCommandReponse);
        }
        private async Task AssignImage(BookFileRequestDto bookFileDto)
        {
            bookFileDto.Book.Image = await _fileService.GetFileContent(bookFileDto.File);
            bookFileDto.Book.UploadImageDirectory = Path.Combine(_environment.ContentRootPath, @"Client", "img");
            bookFileDto.Book.ImageExtension = GetExtension(bookFileDto.File.ContentType);
        }

        private async Task<IActionResult?> ValidateRequest(BookFileRequestDto bookFileDto)
        {
            // Check if the uploaded file is an image with valid size
            if (!_fileService.IsFileValid(bookFileDto.File))
                return BadRequest("Invalid file type/length. Please upload an image with valid type.");

            var image = await _fileService.GetFileContent(bookFileDto.File);
            if (image == null || image.Length == 0)
                return BadRequest("Image is empty/invalid");
            return null;
        }
    }
}
