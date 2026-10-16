using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;

public sealed record AdminDeleteQuizCommand(Guid QuizId) : IRequest<Result>;
