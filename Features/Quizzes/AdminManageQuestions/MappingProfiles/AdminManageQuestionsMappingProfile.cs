using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using Mapster;

namespace exam_system.Features.Quizzes.AdminManageQuestions.MappingProfiles;

public sealed class AdminManageQuestionsMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AdminUpdateQuestionRequest, Question>()
            .IgnoreNullValues(true);
    }
}
