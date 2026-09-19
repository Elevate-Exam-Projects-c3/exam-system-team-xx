using exam_system.Features.Quizzes.AdminCreateQuiz.Requests;
using exam_system.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Commands;

public record AdminCreateQuizCommand(AdminCreateQuizRequest AdminCreateQuizRequest) : IRequest<Result<Unit>>;