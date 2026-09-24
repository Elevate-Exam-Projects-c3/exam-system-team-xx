using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;

public sealed record AdminQuizPublishCheckQuery(Guid QuizId) : IRequest<Result<IReadOnlyList<ChecklistItemResponse>>>;
