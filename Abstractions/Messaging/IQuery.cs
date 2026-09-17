using exam_system.Shared;
using MediatR;

namespace exam_system.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
