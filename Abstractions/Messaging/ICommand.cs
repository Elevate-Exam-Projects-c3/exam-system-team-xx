using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }


}
