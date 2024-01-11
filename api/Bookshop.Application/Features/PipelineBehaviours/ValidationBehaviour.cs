using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookshop.Application.Features.PipelineBehaviours
{
    /// <summary>
    /// ValidationBehaviour: A MediatR pipeline behavior for handling command validation using FluentValidation.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
    public class ValidationBehaviour<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : ICommandResponse
    {
        private readonly IEnumerable<IValidator<TCommand>> _validators;
        private readonly ILogger<ValidationBehaviour<TCommand, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the ValidationBehaviour class.
        /// </summary>
        /// <param name="validators">The collection of validators for the command.</param>
        /// <param name="logger">The logger used for logging.</param>
        public ValidationBehaviour(IEnumerable<IValidator<TCommand>> validators, ILogger<ValidationBehaviour<TCommand, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        /// <summary>
        /// Handles the command validation using FluentValidation.
        /// </summary>
        /// <param name="request">The command being handled.</param>
        /// <param name="next">The delegate representing the next behavior/handler in the pipeline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response produced by the handler.</returns>
        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // Skip validation if no validators are provided
                if (!_validators.Any()) return await next();

                // Create a validation context for the command
                var context = new ValidationContext<TCommand>(request);

                // Asynchronously validate the command using FluentValidation
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

                // Wait for all validation tasks to complete
                var validationResults = await Task.WhenAll(validationTasks);

                // Process failed validations
                var unexpectedFailedResults = validationResults.Where(result => result == null);
                if (unexpectedFailedResults.Any())
                {
                    throw new ValidationException("One or more validation errors occurred.");
                }

                // Aggregate failures from individual validators
                var failures = validationResults?.SelectMany(r => r.Errors)?.Where(f => f != null)?.ToList();

                // Continue with the next behavior/handler if there are no failures
                if (failures is not { Count: > 0 }) return await next();

                // Create an instance of the command response type
                var realResponseType = typeof(TResponse);
                if (Activator.CreateInstance(realResponseType) is not ICommandResponse commandResponse)
                    return await next();

                // Populate the command response with validation errors
                commandResponse.ValidationErrors = new List<string>();
                commandResponse.Success = false;

                // Log and add validation errors to the response
                foreach (var error in failures)
                {
                    _logger?.LogError(error.ErrorMessage);
                    commandResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                return (TResponse)commandResponse;
            }
            catch (Exception ex)
            {
                // Log and rethrow exceptions during handling
                _logger.LogError(ex, "Error handling {RequestType}", typeof(TCommand).Name);
                throw;
            }
        }
    }
}
