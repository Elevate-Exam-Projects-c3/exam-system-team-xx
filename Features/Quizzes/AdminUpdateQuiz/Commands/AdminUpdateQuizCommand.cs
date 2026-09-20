using exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;

public sealed record AdminUpdateQuizCommand(Guid QuizId, AdminUpdateQuizRequest AdminUpdateQuizRequest) : IRequest<Result>;
