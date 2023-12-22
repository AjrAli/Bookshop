using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
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
        public async Task<IActionResult> GetById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetById<BookResponseDto>
            {
                Id = id,
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAll<BookResponseDto>());
            return Ok(dataReponse);
        }
    }
}
