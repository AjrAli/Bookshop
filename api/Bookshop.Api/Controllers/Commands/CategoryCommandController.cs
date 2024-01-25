using Bookshop.Application.Configuration;
using Bookshop.Application.Features.Categories;
using Bookshop.Application.Features.Categories.Commands.CreateCategory;
using Bookshop.Application.Features.Categories.Commands.DeleteCategory;
using Bookshop.Application.Features.Categories.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Commands
{
    [ApiController]
    [Authorize(Roles = RoleNames.Administrator)]
    [Route("api/category")]
    public class CategoryCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto categoryDto)
        {
            var dataCommandReponse = await _mediator.Send(new CreateCategory
            {
                Category = categoryDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequestDto categoryDto)
        {
            var dataCommandReponse = await _mediator.Send(new UpdateCategory
            {
                Category = categoryDto
            });
            return Ok(dataCommandReponse);
        }
        [HttpPost("delete-category")]
        public async Task<IActionResult> DeleteCategory([FromBody] long id)
        {
            var dataCommandReponse = await _mediator.Send(new DeleteCategory
            {
                Id = id
            });
            return Ok(dataCommandReponse);
        }
    }
}
