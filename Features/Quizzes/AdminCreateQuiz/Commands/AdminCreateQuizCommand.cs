using exam_system.Features.Quizzes.AdminCreateQuiz.Requests;
using exam_system.Features.Shared.ResultPattern;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Commands;

public record AdminCreateQuizCommand(AdminCreateQuizRequest AdminCreateQuizRequest) : IRequest<Result>;