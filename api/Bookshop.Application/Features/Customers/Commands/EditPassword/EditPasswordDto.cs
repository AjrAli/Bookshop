using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Customers.Commands.EditPassword
{
    public class EditPasswordDto
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}

