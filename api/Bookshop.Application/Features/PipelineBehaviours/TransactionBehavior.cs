using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookshop.Application.Features.PipelineBehaviours
{
    public class TransactionBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    {
        private readonly BookshopDbContext _context;
        private readonly ILogger<TransactionBehavior<TCommand, TResponse>> _logger;

        public TransactionBehavior(ILogger<TransactionBehavior<TCommand, TResponse>> logger, BookshopDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _logger.LogInformation("Starting a database transaction for {RequestName}", typeof(TCommand).Name);
                var response = await next();
                _logger.LogInformation("Committing the database transaction for {RequestName}", typeof(TCommand).Name);
                if (!request.IsSaveChangesAsyncCalled)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch (InsufficientQuantityException ex)
            {
                //Update just shoppingCart this exception is safe
                _logger.LogError(ex, "ShoppingCart will be updated with correct quantity available");
                await transaction.CommitAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during transaction for {RequestName}", typeof(TCommand).Name);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}