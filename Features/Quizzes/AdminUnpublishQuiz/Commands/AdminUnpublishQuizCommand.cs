using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;

public sealed record AdminUnpublishQuizCommand(Guid QuizId) : IRequest<Result>;
