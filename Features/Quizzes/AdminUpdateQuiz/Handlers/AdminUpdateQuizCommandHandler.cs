using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MapsterMapper;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Handlers;

public sealed class AdminUpdateQuizCommandHandler : IRequestHandler<AdminUpdateQuizCommand, Result>
{
    private readonly IGenericRepository<Quiz> _quizRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminUpdateQuizCommandHandler(IGenericRepository<Quiz> quizRepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _quizRepo = quizRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(AdminUpdateQuizCommand request, CancellationToken cancellationToken)
    {
        // Get the quiz to be updated
        var quizToBeUpdated = await _quizRepo.GetByIdAsync(request.QuizId);

        // Check whether quiz is null
        if (quizToBeUpdated is null)
            return Result.Failure(error: Error.NotFound("Quiz.NotFound", $"Quiz of Id ({request.QuizId}) doesn't exist on the system."));

        // Update the quiz
        quizToBeUpdated = _mapper.Map(request.AdminUpdateQuizRequest,quizToBeUpdated);
        _quizRepo.Update(quizToBeUpdated);

        // Save changes to DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
