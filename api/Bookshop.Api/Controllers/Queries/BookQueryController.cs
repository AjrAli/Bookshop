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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            GetByIdQueryResponse<Book>? dataReponse = await _mediator.Send(new GetByIdQuery<Book>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllQueryResponse<Book>? dataReponse = await _mediator.Send(new GetAllQuery<Book>());
            return Ok(dataReponse);
        }
    }
}
