using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Books.Commands.Comments.DeleteComment
{
    public class DeleteComment : ICommand<CommandResponse>
    {
        public long Id { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
