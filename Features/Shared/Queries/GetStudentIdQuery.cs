using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Shared.Queries;

public sealed record GetStudentIdQuery(Guid UserId) : IRequest<Result<Guid>>;

