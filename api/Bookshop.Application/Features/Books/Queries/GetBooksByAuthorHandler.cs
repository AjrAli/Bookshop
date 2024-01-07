﻿using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Queries;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetBooksByAuthorHandler : IQueryHandler<GetBooksByAuthor, GetAllResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBooksByAuthorHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetBooksByAuthor request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Books.CountAsync(x => x.AuthorId == request.AuthorId, cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(Book)} found");
            }

            var query = _dbContext.Books.Where(x => x.AuthorId == request.AuthorId).AsQueryable();
            query = query.Include(x => x.Author)
                         .Include(x => x.Category);
            var sourceType = typeof(Book);
            var targetType = typeof(BookResponseDto);

            var listDto = await query
                .Select(x => _mapper.Map(x, sourceType, targetType))
                .ToListAsync(cancellationToken: cancellationToken);

            return new GetAllResponse
            {
                ListDto = listDto
            };
        }
    }
}
