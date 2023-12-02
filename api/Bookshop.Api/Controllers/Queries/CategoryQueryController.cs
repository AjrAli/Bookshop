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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(long? id)
        {
            var dataReponse = await _mediator.Send(new GetByIdQuery<Category>
            {
                Id = id
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllQuery<Category>());
            return Ok(dataReponse);
        }
    }
}
