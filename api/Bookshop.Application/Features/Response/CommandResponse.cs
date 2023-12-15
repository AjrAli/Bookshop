using Bookshop.Application.Features.Response.Contracts;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Response
{
    public class CommandResponse : BaseResponse, ICommandResponse
    {
        public CommandResponse() => Success = true;
        public CommandResponse(bool success) { Success = success; }
        public CommandResponse(string message, bool success) : base(message, success) { }

        public CommandResponse(string message, string validationError) : base(message, false)
        {
            ValidationErrors = new List<string>
            {
                validationError
            };
        }
        public CommandResponse(string message, IList<string>? validationErrors)
            : base(message, false)
        {
            ValidationErrors = validationErrors;
        }

        [JsonIgnore]
        public bool IsSaveChangesAsyncCalled { get; set; }
        private IList<string>? _validationErrors;
        public IList<string>? ValidationErrors 
        {
            get => _validationErrors;
            set 
            {
                _validationErrors = value;
                Success = false;
            }
        }
    }
}
