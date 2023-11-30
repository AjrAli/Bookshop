using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllQueryHandler<T> : IQueryHandler<GetAllQuery<T>, GetAllQueryResponse<T>> where T : class
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllQueryHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllQueryResponse<T>> Handle(GetAllQuery<T> request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Set<T>().CountAsync(cancellationToken);
            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(T)} found");
            }

            var listDto = _mapper.Map<List<T>>(await _dbContext.Set<T>().ToListAsync(cancellationToken: cancellationToken));

            return new GetAllQueryResponse<T>
            {
                ListDto = listDto
            };
        }
    }
}
