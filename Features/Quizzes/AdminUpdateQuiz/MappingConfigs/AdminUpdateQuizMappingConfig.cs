using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;
using Mapster;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.MappingConfigs;

public sealed class AdminUpdateQuizMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AdminUpdateQuizRequest, Quiz>()
            .IgnoreNullValues(true);
    }
}
