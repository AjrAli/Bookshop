using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorHandler : ICommandHandler<UpdateAuthor, AuthorCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateAuthorHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AuthorCommandResponse> Handle(UpdateAuthor request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var editedAuthor = await EditAuthorFromDto(request.Author);
            EditAuthorInDatabase(editedAuthor);
            await SaveChangesAsync(cancellationToken);
            var editedAuthorDto = _mapper.Map<AuthorResponseDto>(editedAuthor);
            return new()
            {
                Author = editedAuthorDto,
                Message = $"Author successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private async Task<Author> EditAuthorFromDto(AuthorRequestDto authorDto)
        {
            var authorExisting = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == authorDto.Id);
            authorExisting.Name = authorDto.Name;
            authorExisting.About = authorDto.About;
            return authorExisting;
        }
        private void EditAuthorInDatabase(Author author)
        {
            _dbContext.Authors.Update(author);
        }
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ValidateRequest(UpdateAuthor request)
        {
            if (!await _dbContext.Authors.AnyAsync(x => x.Id == request.Author.Id))
                throw new BadRequestException($"Author: {request.Author.Id} not found in the database.");
        }
    }
}
