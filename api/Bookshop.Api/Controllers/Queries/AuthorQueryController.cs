using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/author")]
    public class AuthorQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorQueryController> _logger;
        public AuthorQueryController(IMediator mediator,
                                ILogger<AuthorQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildAuthorQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetById<Author>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildAuthorQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAll<Author>()
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<Author, object>>, List<Expression<Func<object, object>>>> BuildAuthorQueryConfiguration()
        {
            return new()
            {
                {x => x.Books, new List<Expression<Func<object, object>>>(){ y => (y as Book).Category} }
            };
        }
    }
}
