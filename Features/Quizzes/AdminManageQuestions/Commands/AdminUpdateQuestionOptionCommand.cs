using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public sealed record AdminUpdateQuestionOptionCommand(Guid OptionId, string OptionText) : IRequest<Result>;