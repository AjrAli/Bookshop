using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Categories.Queries
{
    public class GetCategoryByIdHandler : IQueryHandler<GetCategoryById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetCategoryById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categories.AsQueryable();
            query = query.Include(x => x.Books);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(Category)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(Category);
            var targetType = typeof(CategoryResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
