﻿using Bookshop.Application.Features.Search.Queries.GetSearchResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
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

        [HttpGet]
        [Route("{keyword?}")]
        public async Task<IActionResult> GetSearchResults(string keyword)
        {
            var dataReponse = await _mediator.Send(new GetSearchResultsQuery
            {
                Keyword = keyword
            });
            return Ok(dataReponse);
        }
    }
}
