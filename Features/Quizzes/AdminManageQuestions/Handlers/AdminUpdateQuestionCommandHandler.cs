using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MapsterMapper;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public sealed class AdminUpdateQuestionCommandHandler : IRequestHandler<AdminUpdateQuestionCommand, Result>
{
    private readonly IGenericRepository<Question> _questionRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminUpdateQuestionCommandHandler(IGenericRepository<Question> questionRepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _questionRepo = questionRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(AdminUpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        // Get the question to be updated
        var questionToBeUpdated = await _questionRepo.GetByIdAsync(request.QuestionId, cancellationToken);

        // Check whether the returned question is null
        if (questionToBeUpdated is null)
            return Result.Failure(error: Error.NotFound("Question.NotFound", $"Question of Id ({request.QuestionId}) doesn't exist on the system."));
        
        // Check whether the updated value of OrderIndex of the question already exists under same quiz (if any)
        if(request.AdminUpdateQuestionRequest.OrderIndex.HasValue &&
            await _questionRepo.AnyAsync(question => question.QuizId == questionToBeUpdated.QuizId &&
                question.OrderIndex == request.AdminUpdateQuestionRequest.OrderIndex &&
                question.OrderIndex != questionToBeUpdated.OrderIndex))
            return Result.Failure(Error.Validation("Question.ExistingOrderIndex", "Order index is already assigned to other existing question under the same quiz."));

        // Update the question
        var questionUpdate = request.AdminUpdateQuestionRequest;
        questionUpdate.Text = questionUpdate.Text?.Trim();
        questionUpdate.Explanation = questionUpdate.Explanation?.Trim();
        questionToBeUpdated = _mapper.Map(questionUpdate, questionToBeUpdated);
        _questionRepo.Update(questionToBeUpdated);

        // Save changes on DB
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
