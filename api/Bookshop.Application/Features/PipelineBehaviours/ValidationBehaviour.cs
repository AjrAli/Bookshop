using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookshop.Application.Features.PipelineBehaviours
{
    public class ValidationBehaviour<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : ICommandResponse
    {
        private readonly IEnumerable<IValidator<TCommand>> _validators;
        private readonly ILogger<ValidationBehaviour<TCommand, TResponse>> _logger;
        public ValidationBehaviour(IEnumerable<IValidator<TCommand>> validators, ILogger<ValidationBehaviour<TCommand, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                if (!_validators.Any()) return await next();
                var context = new ValidationContext<TCommand>(request);
                var validationTasks = _validators
                                                .Select(v => v.ValidateAsync(context, cancellationToken)
                                                    .ContinueWith(task =>
                                                    {
                                                        if (task.Exception != null)
                                                        {
                                                            // Log or handle the exception here
                                                            _logger?.LogError(task.Exception, "Error in validation task");
                                                            return null;
                                                        }

                                                        return task.Result;
                                                    }, cancellationToken))
                                                .ToList();

                var validationResults = await Task.WhenAll(validationTasks);

                // Process failed validations
                var unExpectedfailedResults = validationResults.Where(result => result == null);
                if (unExpectedfailedResults.Any())
                {
                    throw new ValidationException("One or more validation errors occurred.");
                }

                var failures = validationResults?.SelectMany(r => r.Errors)?.Where(f => f != null)?.ToList();
                if (failures is not { Count: > 0 }) return await next();
                var realResponseType = typeof(TResponse);
                if (Activator.CreateInstance(realResponseType) is not ICommandResponse errorResponse)
                    return await next();
                errorResponse.ValidationErrors = new List<string>();
                foreach (var error in failures)
                {
                    _logger?.LogError(error.ErrorMessage);
                    errorResponse.ValidationErrors.Add(error.ErrorMessage);
                }
                return (TResponse)errorResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {RequestType}", typeof(TCommand).Name);
                throw;
            }
        }
    }
}
