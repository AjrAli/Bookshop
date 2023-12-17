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
        private readonly ILogger<SearchQueryController> _logger;
        public SearchQueryController(IMediator mediator,
                            ILogger<SearchQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("book/{keyword}")]
        public async Task<IActionResult> GetSearchResults(string keyword)
        {
            var dataReponse = await _mediator.Send(new GetSearchResults
            {
                Keyword = keyword
            });
            return Ok(dataReponse);
        }
    }
}
