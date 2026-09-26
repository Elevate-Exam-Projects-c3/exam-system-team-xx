using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Shared.Contracts;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;

public sealed record AdminPublishQuizOrhestrator(Guid QuizId) : IRequest<Result>, ITransactional;