using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
