using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBook : ICommand<BookCommandResponse>
    {
        public BookRequestDto? Book { get; set; }
        public long? Id { get; set; }
    }
}
