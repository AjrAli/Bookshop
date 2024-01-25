using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

namespace Bookshop.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : ICommandHandler<CreateCategory, CategoryCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryCommandResponse> Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            var newCategory = CreateNewCategoryFromDto(request.Category);
            await StoreCategoryInDatabase(newCategory, cancellationToken);
            var newCategoryCreated = _mapper.Map<CategoryResponseDto>(newCategory);
            return new CategoryCommandResponse()
            {
                Category = newCategoryCreated,
                Message = $"Category successfully created",
                IsSaveChangesAsyncCalled = true
            };
        }

        private async Task StoreCategoryInDatabase(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private Category CreateNewCategoryFromDto(CategoryRequestDto categoryDtok)
        {
            return new Category(categoryDtok.Title, categoryDtok.Description);
        }

        public Task ValidateRequest(CreateCategory request)
        {
            throw new NotImplementedException();
        }
    }
}
