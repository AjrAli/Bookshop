﻿using Bookshop.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Api.Controllers.Queries
{
    [ApiController]
    [Route("api/books")]
    public class BookQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var dataReponse = await _mediator.Send(new GetBookById
            {
                Id = id,
            });
            return Ok(dataReponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataReponse = await _mediator.Send(new GetAllBooks());
            return Ok(dataReponse);
        }
        [HttpGet("by-author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthorId(long authorId)
        {
            var dataResponse = await _mediator.Send(new GetBooksByAuthor
            {
                AuthorId = authorId
            });
            return Ok(dataResponse);
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategoryId(long categoryId)
        {
            var dataResponse = await _mediator.Send(new GetBooksByCategory
            {
                CategoryId = categoryId
            });
            return Ok(dataResponse);
        }
    }
}
