using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Shared.Queries;

public sealed record CheckStudentIsEnrolledInDiplomaQuery(Guid StudentId, Guid DiplomaId) : IRequest<Result>;

