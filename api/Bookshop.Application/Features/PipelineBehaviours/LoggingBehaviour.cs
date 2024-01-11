using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Bookshop.Application.Features.PipelineBehaviours
{
    /// <summary>
    /// LoggingBehaviour: A MediatR pipeline behavior for logging requests and responses.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the LoggingBehaviour class.
        /// </summary>
        /// <param name="logger">The logger used for logging.</param>
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the request, logs information, and continues with the pipeline.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="next">The delegate representing the next behavior/handler in the pipeline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response produced by the handler.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                // Log information about handling the request
                _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);

                // Log information about properties of the request
                var myTypeRequest = request?.GetType();
                IList<PropertyInfo>? propsOfMyRequest = myTypeRequest?.GetProperties()?.ToList();
                if (propsOfMyRequest != null)
                {
                    foreach (var prop in propsOfMyRequest)
                        _logger.LogInformation(
                            $"Property Name : {prop?.Name}, Property Type : {prop?.PropertyType}");
                }

                // Continue with the next behavior/handler in the pipeline
                var response = await next();

                // Log information about handling being completed
                _logger.LogInformation("Handled {RequestType}", typeof(TRequest).Name);

                return response;
            }
            catch (Exception ex)
            {
                // Log an error if an exception occurs during handling
                _logger.LogError(ex, "Error handling {RequestType}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}
