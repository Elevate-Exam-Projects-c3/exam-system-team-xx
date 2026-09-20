namespace exam_system.Features.Shared.Contracts;

/// <summary>
/// Defines a contract for components that participate in transactions.
/// </summary>
/// <remarks>
/// Implementations control transactional boundaries and enable commit and rollback semantics to ensure
/// operations are executed as a single atomic unit.
/// </remarks>
public interface ITransactional
{
}
