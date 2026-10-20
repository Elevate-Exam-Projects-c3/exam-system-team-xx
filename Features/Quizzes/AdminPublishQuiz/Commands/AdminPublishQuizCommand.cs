using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Commands;

public sealed record AdminPublishQuizCommand(Guid QuizId) : IRequest<Result>;