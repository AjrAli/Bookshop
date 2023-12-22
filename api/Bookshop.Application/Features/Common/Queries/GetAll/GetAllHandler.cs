using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllHandler<Dto,T> : IQueryHandler<GetAll<Dto>, GetAllResponse> 
        where Dto : class
        where T : class
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAll<Dto> request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Set<T>().CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(T)} found");
            }

            var query = _dbContext.Set<T>().AsQueryable();

            // Store the constant expressions in local variables
            var sourceType = typeof(T);
            var targetType = typeof(Dto);

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
