using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class CheckDiplomaExistsQueryHandler : IRequestHandler<CheckDiplomaExistsQuery, Result>
{
    private readonly IGenericRepository<Diploma> _diplomaRepo;

    public CheckDiplomaExistsQueryHandler(IGenericRepository<Diploma> diplomaRepo)
    {
        _diplomaRepo = diplomaRepo;
    }

    public async Task<Result> Handle(CheckDiplomaExistsQuery request, CancellationToken cancellationToken)
    {
        // Check if diploma whose Id is passed exists
        var diplomaExists = await _diplomaRepo.AnyAsync(d => d.Id == request.DiplomaId);

        if (!diplomaExists)
            return Result.Failure(error: new ("Diploma.NotExist",$"Diploma of Id ({request.DiplomaId}) doesn't exist on the system.",ErrorType.NotFound));

        return Result.Success();
    }
}
