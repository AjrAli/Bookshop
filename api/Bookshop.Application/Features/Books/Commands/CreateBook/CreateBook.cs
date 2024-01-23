using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Books.Commands.CreateBook
{
    public class CreateBook : ICommand<BookCommandResponse>
    {
        public BookRequestDto? Book { get; set; }
    }
}
