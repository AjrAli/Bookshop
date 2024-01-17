using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response.Contracts;
using Bookshop.Domain.Exceptions;
using Bookshop.Persistence.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookshop.Application.Features.PipelineBehaviours
{
    /// <summary>
    /// TransactionBehavior: A MediatR pipeline behavior for handling transactions in commands.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
    public class TransactionBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : ICommandResponse
    {
        private readonly BookshopDbContext _context;
        private readonly ILogger<TransactionBehavior<TCommand, TResponse>> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Initializes a new instance of the TransactionBehavior class.
        /// </summary>
        /// <param name="logger">The logger used for logging.</param>
        /// <param name="context">The database context used for transactions.</param>
        public TransactionBehavior(ILogger<TransactionBehavior<TCommand, TResponse>> logger, BookshopDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Handles the command, starts a database transaction, and commits or rolls back based on the result.
        /// </summary>
        /// <param name="request">The command being handled.</param>
        /// <param name="next">The delegate representing the next behavior/handler in the pipeline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response produced by the handler.</returns>
        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            //SemaphoreSlim limits the number of threads that can access a resource or pool of resources concurrently
            //In this case is to ensure that only one transaction is processed at a time per thread
            await _semaphore.WaitAsync(cancellationToken);
            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _logger.LogInformation("Starting a database transaction for {RequestName}", typeof(TCommand).Name);
                var response = await next();
                _logger.LogInformation("Committing the database transaction for {RequestName}", typeof(TCommand).Name);

                // Save changes only if SaveChangesAsync was not called within the handler
                if (!response.IsSaveChangesAsyncCalled)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch (InsufficientBookQuantityException ex)
            {
                // Handle a specific exception and commit the transaction (safe operation)
                _logger.LogError(ex, "ShoppingCart will be updated with correct quantity available");
                await transaction.CommitAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                // Handle generic exceptions, log error, rollback the transaction, and rethrow
                _logger.LogError(ex, "Error occurred during transaction for {RequestName}", typeof(TCommand).Name);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                // Dispose of the transaction
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
                _semaphore.Release();
            }
        }
    }
}
