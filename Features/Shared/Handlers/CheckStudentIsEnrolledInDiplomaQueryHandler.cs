using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Shared.Handlers;

public sealed class CheckStudentIsEnrolledInDiplomaQueryHandler : IRequestHandler<CheckStudentIsEnrolledInDiplomaQuery, Result>
{
    private readonly IGenericRepository<StudentEnrollment> _studentEnrollmentRepo;

    public CheckStudentIsEnrolledInDiplomaQueryHandler(IGenericRepository<StudentEnrollment> studentEnrollmentRepo)
    {
        _studentEnrollmentRepo = studentEnrollmentRepo;
    }

    public async Task<Result> Handle(CheckStudentIsEnrolledInDiplomaQuery request, CancellationToken cancellationToken)
    {
        // Check whether student is enrolled in a specific diploma
        var isStudentInDiploma = await _studentEnrollmentRepo.AnyAsync(sr => sr.DiplomaId == request.DiplomaId 
        && sr.StudentId == request.StudentId, cancellationToken);

        if (!isStudentInDiploma)
            return Result.Failure(Error.NotFound("StudentEnrollment.NotFound",$"Student ({request.StudentId} is not enrolled in diploma ({request.DiplomaId})"));

        return Result.Success();
    }
}
