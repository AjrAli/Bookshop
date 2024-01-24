using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Authors.Queries
{
    public class GetAuthorByIdHandler : IQueryHandler<GetAuthorById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAuthorByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetAuthorById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Authors.AsQueryable();
            query = query.Include(x => x.Books);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(Author)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(Author);
            var targetType = typeof(AuthorResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
