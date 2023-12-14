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
    [Route("category")]
    public class CategoryQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryQueryController> _logger;
        public CategoryQueryController(IMediator mediator,
                                ILogger<CategoryQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var queryConfig = BuildCategoryQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetById<Category>
            {
                Id = id,
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var queryConfig = BuildCategoryQueryConfiguration();
            var dataReponse = await _mediator.Send(new GetAll<Category>
            {
                NavigationPropertyConfigurations = queryConfig
            });
            return Ok(dataReponse);
        }
        private Dictionary<Expression<Func<Category, object>>, List<Expression<Func<object, object>>>> BuildCategoryQueryConfiguration()
        {
            return new()
            {
                {x => x.Books, new List<Expression<Func<object, object>>>(){ y => (y as Book).Author} }
            };
        }
    }
}
