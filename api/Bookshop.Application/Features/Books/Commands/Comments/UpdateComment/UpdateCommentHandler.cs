using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Commands.Comments.UpdateComment
{
    public class UpdateCommentHandler : ICommandHandler<UpdateComment, CommentCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCommentHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CommentCommandResponse> Handle(UpdateComment request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var editedComment = await EditCommentFromDto(request.Comment);
            EditCommentInDatabase(editedComment);
            await SaveChangesAsync(cancellationToken);
            var editedCommentDto = _mapper.Map<CommentResponseDto>(editedComment);
            return new()
            {
                Comment = editedCommentDto,
                Message = $"Comment successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private async Task<Comment> EditCommentFromDto(CommentRequestDto commentDto)
        {
            var commentExisting = await _dbContext.Comments.Include(x => x.Customer).ThenInclude(x => x.IdentityData).FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == commentDto.UserId &&
                                                                                                              x.Id == commentDto.Id);
            commentExisting.Title = commentDto.Title;
            commentExisting.Content = commentDto.Content;
            commentExisting.Rating = commentDto.Rating;
            return commentExisting;
        }
        private void EditCommentInDatabase(Comment comment)
        {
            _dbContext.Comments.Update(comment);
        }
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ValidateRequest(UpdateComment request)
        {
            if (!await _dbContext.Comments.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == request.Comment.UserId &&
                                                                                  x.Id == request.Comment.Id))
                throw new BadRequestException($"Comment: {request.Comment.Id} not found in the database.");
        }
    }
}
