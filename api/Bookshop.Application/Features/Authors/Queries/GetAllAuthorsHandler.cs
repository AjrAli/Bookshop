using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Authors;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Authors.Queries
{
    public class GetAllAuthorsHandler : IQueryHandler<GetAllAuthors, GetAllResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllAuthorsHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAllAuthors request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Authors.CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(Author)} found");
            }

            var query = _dbContext.Authors.AsQueryable();
            query = query.Include(x => x.Books);
            var sourceType = typeof(Author);
            var targetType = typeof(AuthorResponseDto);

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
