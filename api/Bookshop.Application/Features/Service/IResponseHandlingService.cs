using Bookshop.Application.Features.Response;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
namespace Bookshop.Application.Features.Service
{
    public interface IResponseHandlingService
    {
        void ValidateRequestResult(ILogger logger, IBaseResponse baseResponse, ValidationResult validationResult);
    }
}
