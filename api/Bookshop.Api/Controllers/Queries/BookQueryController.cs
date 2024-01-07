using Bookshop.Application.Features.Books.Queries;
using Bookshop.Application.Features.Common.Queries.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/book")]
    public class BookQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookQueryController> _logger;
        public BookQueryController(IMediator mediator,
                                ILogger<BookQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetBookById
            {
                Id = id,
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllBooks());
            return Ok(dataReponse);
        }
        [HttpGet("author/{id}")]
        public async Task<IActionResult> GetBooksByAuthorId(long id)
        {
            var dataReponse = await _mediator.Send(new GetBooksByAuthor
            {
                AuthorId = id,
            });
            return Ok(dataReponse);
        }
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetBooksByCategoryId(long id)
        {
            var dataReponse = await _mediator.Send(new GetBooksByCategory
            {
                CategoryId = id,
            });
            return Ok(dataReponse);
        }
    }
}
