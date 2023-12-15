using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Exceptions
{
    public class ValidationException : BaseException
    {
        private readonly IList<string>? _validationErrors;
        public IList<string>? ValidationErrors { get { return _validationErrors; } }
        public ValidationException(string message, IList<string> validationErrors) : base(message)
        {
            _validationErrors = validationErrors;
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationErrorResponse CreateValidationErrorResponse(string? newValidationErrorStr = null)
        {
            if (newValidationErrorStr != null)
            {
                return new(Message, newValidationErrorStr);
            }
            return new(Message, ValidationErrors);
        }
    }
    public class ValidationErrorResponse : ErrorResponse
    {
        public IList<string>? ValidationErrors { get; set; }
        public ValidationErrorResponse(string message) : base(message)
        {
        }
        public ValidationErrorResponse(string message, IList<string>? validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }
        public ValidationErrorResponse(string message, string validationError) : base(message)
        {
            ValidationErrors = new List<string>
            {
                validationError
            };
        }
    }
}
