using Bookshop.Application.Features.Common.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    //[Authorize]
    [Route("api/category")]
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
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetCategoryById
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllCategories());
            return Ok(dataReponse);
        }
    }
}
