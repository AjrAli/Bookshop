using Bookshop.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryQueryController> _logger;
        public CategoryQueryController(IMediator mediator)
        {
            _mediator = mediator;
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
