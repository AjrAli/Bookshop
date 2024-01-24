using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Categories.Queries
{
    public class GetAllCategoriesHandler : IQueryHandler<GetAllCategories, GetAllResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAllCategories request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Categories.CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(Category)} found");
            }

            var query = _dbContext.Categories.AsQueryable();
            query = query.Include(x => x.Books);
            var sourceType = typeof(Category);
            var targetType = typeof(CategoryResponseDto);

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
