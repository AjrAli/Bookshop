using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Books.Commands.DeleteBook
{
    public class DeleteBook : ICommand<CommandResponse>
    {
        public long Id { get; set; }
        [JsonIgnore]
        public string? UploadImageDirectory { get; set; }
    }
}
