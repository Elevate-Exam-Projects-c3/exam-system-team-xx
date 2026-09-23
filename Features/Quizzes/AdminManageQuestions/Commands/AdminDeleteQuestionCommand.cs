using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public sealed record AdminDeleteQuestionCommand(Guid QuestionId) : IRequest<Result>;

