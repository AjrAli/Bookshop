using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

namespace Bookshop.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthorHandler : ICommandHandler<CreateAuthor, AuthorCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateAuthorHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AuthorCommandResponse> Handle(CreateAuthor request, CancellationToken cancellationToken)
        {
            var newAuthor = CreateNewAuthorFromDto(request.Author);
            await StoreAuthorInDatabase(newAuthor, cancellationToken);
            var newAuthorCreated = _mapper.Map<AuthorResponseDto>(newAuthor);
            return new AuthorCommandResponse()
            {
                Author = newAuthorCreated,
                Message = $"Author successfully created",
                IsSaveChangesAsyncCalled = true
            };
        }

        private async Task StoreAuthorInDatabase(Author author, CancellationToken cancellationToken)
        {
            await _dbContext.Authors.AddAsync(author, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private Author CreateNewAuthorFromDto(AuthorRequestDto authorDtok)
        {
            return new Author(authorDtok.Name, authorDtok.About);
        }

        public Task ValidateRequest(CreateAuthor request)
        {
            throw new NotImplementedException();
        }
    }
}
