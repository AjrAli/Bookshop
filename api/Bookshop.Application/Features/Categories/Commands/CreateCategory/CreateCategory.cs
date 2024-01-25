using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategory : ICommand<CategoryCommandResponse>
    {
        public CategoryRequestDto? Category { get; set; }
    }
}
