using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Shared.Queries;

public sealed record GetQuizToStartNewAttemptQuery(Guid QuizId) : IRequest<Result<StartAttemptQuizResponse>>;
