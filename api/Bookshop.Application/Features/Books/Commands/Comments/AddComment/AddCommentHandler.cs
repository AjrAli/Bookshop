using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Commands.Comments.AddComment
{
    public class AddCommentHandler : ICommandHandler<AddComment, CommentCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddCommentHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CommentCommandResponse> Handle(AddComment request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var customerRetrieved = await _dbContext.Customers.FirstOrDefaultAsync(x => x.IdentityUserDataId == request.Comment.UserId);
            var bookRetrieved = await _dbContext.Books.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == request.Comment.BookId);
            var newComment = CreateNewCommentFromDto(request.Comment, customerRetrieved, bookRetrieved);
            await StoreCommentInDatabase(newComment, bookRetrieved, cancellationToken);
            var newCommentCreated = _mapper.Map<CommentResponseDto>(newComment);
            return new CommentCommandResponse()
            {
                Comment = newCommentCreated,
                Message = $"Comment successfully created",
                IsSaveChangesAsyncCalled = true
            };
        }

        private async Task StoreCommentInDatabase(Comment comment, Book book, CancellationToken cancellationToken)
        {
            await _dbContext.Comments.AddAsync(comment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private Comment CreateNewCommentFromDto(CommentRequestDto commentDto, Customer customer, Book book)
        {
            return new Comment(commentDto.Title, commentDto.Content, commentDto.Rating, customer, book);
        }

        public async Task ValidateRequest(AddComment request)
        {
            if (!await _dbContext.Books.AnyAsync(x => x.Id == request.Comment.BookId))
                throw new BadRequestException($"BookId: {request.Comment.BookId} not found in the database.");
            if (await _dbContext.Books.Include(x => x.Comments)
                                      .ThenInclude(x => x.Customer)
                                      .AnyAsync(x => x.Id == request.Comment.BookId && x.Comments.Any(y => y.Customer.IdentityUserDataId == request.Comment.UserId)))
                throw new BadRequestException($"User already posted a comment for this book {request.Comment.BookId}!");
        }
    }
}
