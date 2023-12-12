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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildBookQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetById<Book>
            {
                Id = id,
                NavigationPropertyConfigurations =queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildBookQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAll<Book>
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<Book, object>>, List<Expression<Func<object, object>>>> BuildBookQueryConfiguration()
        {
            return new Dictionary<Expression<Func<Book, object>>, List<Expression<Func<object, object>>>>
            {
                {x => x.Author, null },
                {x => x.Category, null }
            };
        }
    }
}
