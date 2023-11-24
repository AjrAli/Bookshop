using Bookshop.Application.Features.Response;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Bookshop.Application.Features.Service
{
    public class ResponseHandlingService : IResponseHandlingService
    {
        public void ValidateRequestResult(ILogger logger, IBaseResponse baseResponse, ValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                baseResponse.Success = false;
                baseResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    logger.LogError(error.ErrorMessage);
                    baseResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
        }
    }
}
