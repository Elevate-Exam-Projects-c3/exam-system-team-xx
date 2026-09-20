using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Shared.Queries;

public sealed record CheckDiplomaExistsQuery(Guid DiplomaId) : IRequest<Result>;


