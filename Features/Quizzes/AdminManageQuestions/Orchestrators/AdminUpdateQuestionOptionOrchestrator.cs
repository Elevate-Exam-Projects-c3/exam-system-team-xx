using exam_system.Features.Shared.Contracts;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public sealed record AdminUpdateQuestionOptionOrchestrator(Guid QuestionId, Guid OptionId, string OptionText) : IRequest<Result>, ITransactional;
