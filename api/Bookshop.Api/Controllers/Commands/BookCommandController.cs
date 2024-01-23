using Bookshop.Api.Utility.FileRequest;
using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Books.Commands.Comments.AddComment;
using Bookshop.Application.Features.Books.Commands.Comments.DeleteComment;
using Bookshop.Application.Features.Books.Commands.Comments.UpdateComment;
using Bookshop.Application.Features.Books.Commands.CreateBook;
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
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        public BookCommandController(IMediator mediator,
                                ILoggedInUserService loggedInUserService,
                                IConfiguration configuration,
                                IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _loggedInUserService = loggedInUserService;
            _configuration = configuration;
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

        [HttpPost("create-book")]
        public async Task<IActionResult> CreateBook([FromForm] BookFileRequestDto bookFileDto)
        {
            if (bookFileDto.Book == null)
                return BadRequest("BookDto is null.");
            // Check if the uploaded file is an image with valid size
            if (!IsFileValid(bookFileDto.File))
                return BadRequest("Invalid file type/length. Please upload an image with valid type.");

            byte[]? fileContents = null;
            using (var stream = new MemoryStream())
            {
                await bookFileDto.File.CopyToAsync(stream);
                fileContents = stream.ToArray();
            }
            if (fileContents == null || fileContents.Length == 0)
                return BadRequest("Image is empty/invalid");

            bookFileDto.Book.UserId = _loggedInUserService?.GetUserId();
            bookFileDto.Book.Image = fileContents;
            bookFileDto.Book.UploadImageDirectory = Path.Combine(_environment.ContentRootPath, @"Client", "img");
            bookFileDto.Book.ImageExtension = GetExtension(bookFileDto.File.ContentType);
            var dataCommandReponse = await _mediator.Send(new CreateBook
            {
                Book = bookFileDto.Book
            });
            return Ok(dataCommandReponse);
        }
        private bool IsFileValid(IFormFile? file)
        {
            if (file == null ||
                !file.ContentType.StartsWith("image/") ||
                !IsFileSignatureValid(file) ||
                !int.TryParse(_configuration.GetSection("FileSizeLimit")?.Value, out int fileSizeLimit) ||
                file.Length > fileSizeLimit ||
                file.Length == 0)
                return false;
            return true;
        }
        private bool IsFileSignatureValid(IFormFile file)
        {
            // Define valid file signatures for allowed file types
            var allowedFileSignatures = new Dictionary<string, byte[]>
            {
                { "image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 } }, // JPEG
                { "image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } }, // PNG
                { "image/gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } } // GIF
            };

            // Read the first few bytes of the file to check the signature
            using (var stream = file.OpenReadStream())
            {
                byte[] fileSignature = new byte[4];
                stream.Read(fileSignature, 0, 4);

                // Check if the file signature matches the expected signature
                foreach (var allowedSignature in allowedFileSignatures)
                {
                    if (fileSignature.Take(allowedSignature.Value.Length).SequenceEqual(allowedSignature.Value))
                    {
                        return true; // Valid signature found
                    }
                }
            }
            return false; // No valid signature found
        }
    }
}
