using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Commands;

public sealed record StartAttemptCommand(Guid StudentId, Guid QuizId, int QuizDurationInMinutes) : IRequest<Result>;