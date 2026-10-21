using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Shared.Contracts;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Orchestrators;

public sealed record StartAttemptOrchestrator(Guid UserId, Guid QuizId) : IRequest<Result<StartAttemptQuizResponse>>, ITransactional;
