using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public sealed record AdminSetCorrectQuestionOptionCommand(Guid QuestionId, Guid OptionId) : IRequest<Result>;
