using exam_system.Features.Shared.Contracts;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Behaviors;

public class TransactionPipelineBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactional
    where TResponse : Result
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Begin a transaction
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Proceed to request handler
            var response = await next();

            // Check if any errors arised to rollback transaction
            if (response.IsFailure)
            {
                // Rollback transaction
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                // Clear Change In-Memory state
                _unitOfWork.ClearChangeTrackerInMemoryState();

                // Dispose transaction
                _unitOfWork.Dispose();

                return response;
            }

            // Save changes and commit transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Dispose transaction
            _unitOfWork.Dispose();

            return response;
        }
        catch (Exception ex)
        {
            // Rollback transaction
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            // Clear Change In-Memory state
            _unitOfWork.ClearChangeTrackerInMemoryState();

            // Dispose transaction
            _unitOfWork.Dispose();

            throw;
        }
    }
}
