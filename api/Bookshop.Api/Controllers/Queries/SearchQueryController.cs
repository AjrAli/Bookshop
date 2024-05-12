using Bookshop.Application.Features.Search.Queries.GetSearchResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Route("api/search")]
    public class SearchQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SearchQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("book")]
        public async Task<IActionResult> GetSearchResults([FromQuery] string keyword)
        {
            var dataReponse = await _mediator.Send(new GetSearchResults
            {
                Keyword = keyword
            });
            return Ok(dataReponse);
        }
    }
}
