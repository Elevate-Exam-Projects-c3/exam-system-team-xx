using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries;

public sealed record CheckStudentHasInProgressQuizAttemptQuery(Guid StudentId, Guid QuizId) : IRequest<Result>;