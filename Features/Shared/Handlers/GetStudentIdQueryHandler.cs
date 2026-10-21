using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared.Queries;
using exam_system.Features.Shared.ResultPattern;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Shared.Handlers;

public sealed class GetStudentIdQueryHandler : IRequestHandler<GetStudentIdQuery, Result<Guid>>
{
    private readonly IGenericRepository<Student> _studentRepo;

    public GetStudentIdQueryHandler(IGenericRepository<Student> studentRepo)
    {
        _studentRepo = studentRepo;
    }

    public async Task<Result<Guid>> Handle(GetStudentIdQuery request, CancellationToken cancellationToken)
    {
        // Get student id associated with passed user id
        var studentId = await _studentRepo
            .Get(student => student.UserId == request.UserId)
            .Select(student => student.Id)
            .FirstOrDefaultAsync(cancellationToken);

        // Check if student associated with passed user id exists 
        if (studentId == Guid.Empty)
            return Result<Guid>.Failure(error: Error.NotFound("Student.NotFound", $"Student associated with user Id ({request.UserId}) doesn't exist on the system."));
        
        return Result<Guid>.Success(studentId);
    }
}
