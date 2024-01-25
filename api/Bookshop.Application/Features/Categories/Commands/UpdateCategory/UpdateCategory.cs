using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategory : ICommand<CategoryCommandResponse>
    {
        public CategoryRequestDto? Category { get; set; }
    }
}
