using exam_system.Features.Shared.Contracts;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public sealed record AdminSetCorrectQuestionOptionOrchestrator(Guid QuestionId, Guid OptionId) : IRequest<Result>, ITransactional;