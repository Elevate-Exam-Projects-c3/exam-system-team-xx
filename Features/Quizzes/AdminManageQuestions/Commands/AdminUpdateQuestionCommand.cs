using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public sealed record AdminUpdateQuestionCommand(Guid QuestionId, AdminUpdateQuestionRequest AdminUpdateQuestionRequest) : IRequest<Result>;
