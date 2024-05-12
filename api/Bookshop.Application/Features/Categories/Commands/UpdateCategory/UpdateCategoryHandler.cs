using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : ICommandHandler<UpdateCategory, CategoryCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryCommandResponse> Handle(UpdateCategory request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var editedCategory = await EditCategoryFromDto(request.Category, request.Id);
            EditCategoryInDatabase(editedCategory);
            await SaveChangesAsync(cancellationToken);
            var editedCategoryDto = _mapper.Map<CategoryResponseDto>(editedCategory);
            return new()
            {
                Category = editedCategoryDto,
                Message = $"Category successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private async Task<Category> EditCategoryFromDto(CategoryRequestDto categoryDto, long id)
        {
            var categoryExisting = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            categoryExisting.Title = categoryDto.Title;
            categoryExisting.Description = categoryDto.Description;
            return categoryExisting;
        }
        private void EditCategoryInDatabase(Category category)
        {
            _dbContext.Categories.Update(category);
        }
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ValidateRequest(UpdateCategory request)
        {
            if (!await _dbContext.Categories.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Category: {request.Id} not found in the database.");
        }
    }
}
